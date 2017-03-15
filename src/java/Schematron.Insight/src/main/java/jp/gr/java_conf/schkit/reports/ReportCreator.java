package jp.gr.java_conf.schkit.reports;

import jp.gr.java_conf.schkit.enums.ExportFormats;

public class ReportCreator {
    private ExportFormats format = ExportFormats.None;
    private String outPath = "";
    public ReportCreator(){
    }
    public ExportFormats getFormat() {
        return format;
    }
    public void setFormat(ExportFormats format) {
        this.format = format;
    }
    public String getOutPath() {
        return outPath;
    }
    public void setOutPath(String outPath) {
        this.outPath = outPath;
    }

}
