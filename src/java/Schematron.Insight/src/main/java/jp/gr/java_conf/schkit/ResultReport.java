package jp.gr.java_conf.schkit;

public class ResultReport {
    private ResultCollection results = new ResultCollection();
    private int totalSyntaxError = 0;
    private int totalAssert = 0;
    private int totalReport = 0;
    private boolean isFailed = false;
    private String failedMessage = "";

    public ResultCollection getResults() {
        return results;
    }

    public void setResults(ResultCollection results) {
        this.results = results;
        totalSyntaxError = results.sizeOfSyntaxError();
        totalAssert = results.sizeOfAssert();
        totalReport = results.sizeOfReport();
    }

    public int getTotalSyntaxError() {
        return totalSyntaxError;
    }

    public int getTotalAssert() {
        return totalAssert;
    }

    public int getTotalReport() {
        return totalReport;
    }

    public boolean isFailed() {
        return isFailed;
    }

    public void setFailed(boolean isFailed) {
        this.isFailed = isFailed;
    }

    public String getFailedMessage() {
        return failedMessage;
    }

    public void setFailedMessage(String failedMessage) {
        this.failedMessage = failedMessage;
    }

}
