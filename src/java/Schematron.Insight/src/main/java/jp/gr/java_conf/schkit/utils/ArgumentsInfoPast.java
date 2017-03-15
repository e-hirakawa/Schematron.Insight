package jp.gr.java_conf.schkit.utils;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.lang3.StringUtils;

public class ArgumentsInfoPast {
    private String workdir = "";
    private String xmlpath = "";
    private String schpath = "";

    public ArgumentsInfoPast(String[] args) throws Exception {
        Pattern p1 = Pattern.compile("-xsltdir=(.*)");
        Pattern p2 = Pattern.compile("-xmlpath=(.*)");
        Pattern p3 = Pattern.compile("-schpath=(.*)");
        Matcher mc;
        for (String arg : args) {
            mc = p1.matcher(arg);
            if (mc.find()) {
                workdir = mc.group(1);
                continue;
            }
            mc = p2.matcher(arg);
            if (mc.find()) {
                xmlpath = mc.group(1);
                continue;
            }
            mc = p3.matcher(arg);
            if (mc.find()) {
                schpath = mc.group(1);
                continue;
            }
        }
        if(StringUtils.isBlank(workdir))
            throw new Exception("workdir is unset.");
        if(StringUtils.isBlank(xmlpath))
            throw new Exception("xmlpath is unset.");
        if(StringUtils.isBlank(schpath))
            throw new Exception("schpath is unset.");
    }

    public String getWorkdir() {
        return workdir;
    }

    public String getXmlpath() {
        return xmlpath;
    }

    public String getSchpath() {
        return schpath;
    }

}
