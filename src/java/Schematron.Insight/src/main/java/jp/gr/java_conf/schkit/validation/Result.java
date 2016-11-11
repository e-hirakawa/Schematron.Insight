package jp.gr.java_conf.schkit.validation;

import java.nio.file.Path;

public class Result {
	// private variable
	private Path xmlFile;
	private Path schFile;
	private String xmlName;
	private String schName;
	private ResultStatus status;

	// getter setter
	public Path getXmlFile() {
		return xmlFile;
	}

	public void setXmlFile(Path xmlfile) {
		if (this.xmlFile != xmlfile) {
			this.xmlFile = xmlfile;
			this.xmlName = xmlfile.getFileName().toString();
		}
	}

	public Path getSchFile() {
		return schFile;
	}

	public void setSchFile(Path schfile) {
		if (this.schFile != schfile) {
			this.schFile = schfile;
			this.schName = schfile.getFileName().toString();
		}
	}


	public String getXmlName() {
		return xmlName;
	}

	public String getSchName() {
		return schName;
	}

	public ResultStatus getStatus() {
		return status;
	}
	public void setStatus(ResultStatus status) {
		this.status = status;
	}
	public String getStatusName()
	{
		return status.getName();
	}
	public void setStatusName(String name)
	{
		status = ResultStatus.fromName(name);
	}
}
