package jp.gr.java_conf.schkit.utils;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.lang3.StringUtils;

import jp.gr.java_conf.schkit.enums.ExportFormats;

public class ArgumentsInfo {
    private String xmlPath = "";
    private String schPath = "";
    private String outPath = "";
    private ExportFormats format = ExportFormats.None;
    private final Pattern xmlPattern = Pattern.compile("-(x|xml)=(.*)");
    private final Pattern schPattern = Pattern.compile("-(s|sch)=(.*)");
    private final Pattern outPattern = Pattern.compile("-(o|out)=(.*)");
    private final Pattern frmPattern = Pattern.compile("-(f|format)=(.*)");

    public ArgumentsInfo(String[] args) throws FileNotFoundException, IllegalArgumentException {
        Matcher mc;
        for (String arg : args) {
            mc = xmlPattern.matcher(arg);
            if (mc.find()) {
                xmlPath = mc.group(2);
                continue;
            }
            mc = schPattern.matcher(arg);
            if (mc.find()) {
                schPath = mc.group(2);
                continue;
            }
            mc = outPattern.matcher(arg);
            if (mc.find()) {
                outPath = mc.group(2);
                continue;
            }
            mc = frmPattern.matcher(arg);
            if (mc.find()) {
                String val = mc.group(2).trim().toLowerCase();
                switch (val) {
                case "log":
                    format = ExportFormats.Log;
                    break;
                case "tab":
                    format = ExportFormats.Tab;
                    break;
                case "xml":
                    format = ExportFormats.Xml;
                    break;
                case "json":
                    format = ExportFormats.Json;
                    break;
                case "html":
                    format = ExportFormats.Html;
                    break;
                }
                continue;
            }
        }
        if(StringUtils.isBlank(xmlPath))
            throw new IllegalArgumentException("xml path of arguments not set.");
        if(StringUtils.isBlank(schPath))
            throw new IllegalArgumentException("schema path of arguments not set.");
        if(!new File(xmlPath).exists())
            throw new FileNotFoundException(String.format("xml path don't exist of '%s'", xmlPath));
        if(!new File(schPath).exists())
            throw new FileNotFoundException(String.format("schema path don't exist of '%s'", schPath));

        if(format != ExportFormats.None)
        {
            if(StringUtils.isBlank(outPath))
                throw new IllegalArgumentException("out path of arguments not set.");
        }
    }

    public String getXmlPath() {
        return xmlPath;
    }

    public String getSchPath() {
        return schPath;
    }

    public String getOutPath() {
        return outPath;
    }

    public ExportFormats getFormat() {
        return format;
    }
}
