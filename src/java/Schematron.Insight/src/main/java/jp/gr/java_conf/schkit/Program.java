package jp.gr.java_conf.schkit;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.xpath.XPath;
import javax.xml.xpath.XPathConstants;
import javax.xml.xpath.XPathExpressionException;
import javax.xml.xpath.XPathFactory;

import org.apache.commons.io.FilenameUtils;
import org.apache.commons.lang3.StringUtils;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import jp.gr.java_conf.schkit.enums.ResultStatus;
import jp.gr.java_conf.schkit.utils.ArgumentsInfo;
import jp.gr.java_conf.schkit.utils.PositionReportXmlReader;
import net.arnx.jsonic.JSON;

public class Program {
    public static void main(String[] args) {
        ResultReport report = new ResultReport();
        ResultCollection results = new ResultCollection();

        String xsltdir, schpath, xmlpath = "";

        ArgumentsInfo arginfo;
        ProcessorFactory processor;
        try {
            arginfo = new ArgumentsInfo(args);

            processor = new ProcessorFactory();
            processor.Compile(arginfo.getSchPath());

            // validate sch for xml -> c xml stream
            InputStream resultStream = processor.execute(arginfo.getXmlPath());

            results = 
(resultStream, new FileInputStream(xmlpath));
        } catch (TransformerConfigurationException e) {
            report.setFailed(true);
            report.setFailedMessage("Validating Error: Critical configuration error.");
            e.printStackTrace();
        } catch (TransformerException e) {
            report.setFailed(true);
            report.setFailedMessage("Validating Error: Exception status that occurred during the conversion process.");
            e.printStackTrace();
        } catch (Exception e) {
            report.setFailed(true);
            report.setFailedMessage("Validating Error: " + e.getMessage());
            e.printStackTrace();
        }
        try {
            File xmlfile = new File(xmlpath);

            report.setResults(results);
            String json = JSON.encode(report);
            String parent = xmlfile.getParent();
            String name = FilenameUtils.getBaseName(xmlfile.getName());
            Path jsonpath = Paths.get(parent, name + ".json");
            Files.write(jsonpath, json.getBytes());
        } catch (Exception e) {
            report.setFailed(true);
            report.setFailedMessage("Result Report Writing Error: " + e.getMessage());
        }
    }

    private static ResultCollection toResultCollection(InputStream xmlResult, InputStream xmlSource)
            throws SAXException, IOException, ParserConfigurationException, XPathExpressionException {
        ResultCollection results = new ResultCollection();
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        factory.setIgnoringComments(true);
        factory.setNamespaceAware(true);

        Document docResult = factory.newDocumentBuilder().parse(xmlResult);
        Document docSource = PositionReportXmlReader.readXML(xmlSource);

        XPath xpath = XPathFactory.newInstance().newXPath();

        String query1 = "//*[local-name()='failed-assert' or local-name()='successful-report']";
        String query2 = ".//*[local-name()='text']";
        NodeList entries = (NodeList) xpath.evaluate(query1, docResult, XPathConstants.NODESET);
        for (int i = 0; i < entries.getLength(); i++) {
            Result result = new Result();
            Element item = (Element) entries.item(i);
            // set status
            String tagname = item.getLocalName();
            ResultStatus status = tagname.equals("failed-assert") ? ResultStatus.Assert : ResultStatus.Report;
            result.setStatus(status);
            // set test
            if (item.hasAttribute("test"))
                result.setTest(item.getAttribute("test"));
            // set location
            String location = "";
            if (item.hasAttribute("location"))
                location = item.getAttribute("location");
            result.setLocation(location);
            // set role
            if (item.hasAttribute("role"))
                result.setRole(item.getAttribute("role"));
            // set message
            StringBuilder sb = new StringBuilder();
            NodeList textnodes = (NodeList) xpath.evaluate(query2, item, XPathConstants.NODESET);
            for (int j = 0; j < textnodes.getLength(); j++) {
                Node textnode = textnodes.item(j);
                String content = textnode.getTextContent();
                if (StringUtils.isNotEmpty(content)) {
                    if (sb.length() > 0)
                        sb.append("\n");
                    sb.append(content);
                }
            }
            result.setMessage(sb.toString());
            // set line number
            if (StringUtils.isNotEmpty(location)) {
                Node entry = (Node) xpath.evaluate(location, docSource, XPathConstants.NODE);
                if (entry != null) {
                    Object linenumber = entry.getUserData("lineNumber");
                    result.setLine(parseInt(linenumber));
                }
            }
            results.add(result);
        }
        return results;
    }

    private static int parseInt(Object o) {
        int n = -1;
        if (o != null) {
            String s = o.toString();
            Matcher m = Pattern.compile("^([0-9]+)$").matcher(s);
            if (m.find()) {
                n = Integer.parseInt(m.group(1), 10);
            }
        }
        return n;
    }
}
