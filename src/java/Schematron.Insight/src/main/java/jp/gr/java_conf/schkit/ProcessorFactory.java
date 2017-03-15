package jp.gr.java_conf.schkit;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.InputStream;

import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;

import jp.gr.java_conf.schkit.utils.ApplicationInfo;

public class ProcessorFactory {
    private final String xslt1 = "iso_dsdl_include.xsl";
    private final String xslt2 = "iso_abstract_expand.xsl";
    private final String xslt3 = "iso_svrl_for_xslt2.xsl";

    private TransformerFactory factory = null;
    private Transformer[] preTransformers = new Transformer[3];
    private Transformer transformer = null;

    public ProcessorFactory() throws TransformerException {
        factory = TransformerFactory.newInstance();
        preTransformers[0] = createTransformer(factory, xslt1);
        preTransformers[1] = createTransformer(factory, xslt2);
        preTransformers[2] = createTransformer(factory, xslt3);
    }

    public void Compile(String schPath) throws TransformerException {
        try {
            StreamSource sch = new StreamSource(new File(schPath));
            for (Transformer ptf : preTransformers) {
                sch = transform(ptf, sch);
            }
            transformer = factory.newTransformer(sch);
        } catch (TransformerException e) {
            throw new TransformerException(String.format("ProcessorFactory:Compile failed of '%s'", schPath));
        }
    }

    public InputStream execute(String xmlPath) throws TransformerException {
        InputStream is = null;
        try {
            StreamSource xml = new StreamSource(new File(xmlPath));

            ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
            StreamResult streamResult = new StreamResult(outputStream);

            transformer.transform(xml, streamResult);
            byte[] bytes = outputStream.toByteArray();
            is = new ByteArrayInputStream(bytes);
        } catch (TransformerException e) {
            throw new TransformerException(String.format("ProcessorFactory:Execute failed of '%s'", xmlPath));
        }

        return is;
    }

    private Transformer createTransformer(TransformerFactory factory, String xslt) throws TransformerException {
        Transformer transformer = null;
        try {
            InputStream is = ApplicationInfo.getResourceStream(xslt);
            transformer = factory.newTransformer(new StreamSource(is));
        } catch (TransformerConfigurationException e) {
            throw new TransformerException(String.format("ProcessorFactory:Transform failed of '%s'", xslt));
        }
        return transformer;
    }

    private static StreamSource transform(Transformer transform, StreamSource xmlsrc) throws TransformerException {

        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
        StreamResult streamResult = new StreamResult(outputStream);

        transform.transform(xmlsrc, streamResult);
        byte[] bytes = outputStream.toByteArray();

        InputStream is = new ByteArrayInputStream(bytes);
        StreamSource smout = new StreamSource(is);
        return smout;
    }

}
