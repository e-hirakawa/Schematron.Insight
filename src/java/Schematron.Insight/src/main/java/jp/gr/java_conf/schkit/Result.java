package jp.gr.java_conf.schkit;

import java.nio.file.Path;

import javax.management.relation.Role;

import jp.gr.java_conf.schkit.enums.ResultStatus;

/**
 * Validation Result Class
 *
 */
public class Result {
	// private variable
	private Path xmlFile;
	private Path schFile;
	private String xmlName;
	private String schName;
	private ResultStatus status;
	private String location;
	private int line;
	private int pos;
	private String message;
	private String test;
	private Role role;
	// getter setter

	/**
	 * @return validation xml file
	 */
	public Path getXmlFile() {
		return xmlFile;
	}

	/**
	 * @param validation xml file
	 */
	public void setXmlFile(Path xmlfile) {
		if (this.xmlFile != xmlfile) {
			this.xmlFile = xmlfile;
			this.xmlName = xmlfile.getFileName().toString();
		}
	}

	/**
	 * @return schema file
	 */
	public Path getSchFile() {
		return schFile;
	}

	/**
	 * @param schema file
	 */
	public void setSchFile(Path schfile) {
		if (this.schFile != schfile) {
			this.schFile = schfile;
			this.schName = schfile.getFileName().toString();
		}
	}


	/**
	 * @return validation xml file name
	 */
	public String getXmlName() {
		return xmlName;
	}

	/**
	 * @return validation xml file name
	 */
	public String getSchName() {
		return schName;
	}

	/**
	 * @return validation result status
	 */
	public ResultStatus getStatus() {
		return status;
	}
	/**
	 * @param validation result status
	 */
	public void setStatus(ResultStatus status) {
		this.status = status;
	}

	/**
	 * @return validation result status display name
	 */
	public String getStatusName()
	{
		return status.getName();
	}
	/**
	 * @param validation result status display name
	 */
	public void setStatusName(String name)
	{
		status = ResultStatus.fromName(name);
	}

	/**
	 * @return detected xpath location
	 */
	public String getLocation() {
		return location;
	}

	/**
	 * @param detected xpath location
	 */
	public void setLocation(String location) {
		this.location = location;
	}

	/**
	 * @return detected text line number
	 */
	public int getLine() {
		return line;
	}

	/**
	 * @param detected text line number
	 */
	public void setLine(int line) {
		this.line = line;
	}

	/**
	 * @return detected text position
	 */
	public int getPos() {
		return pos;
	}

	/**
	 * @param detected text position
	 */
	public void setPos(int pos) {
		this.pos = pos;
	}

	/**
	 * @return result report text
	 */
	public String getMessage() {
		return message;
	}

	/**
	 * @param result report text
	 */
	public void setMessage(String message) {
		this.message = message;
	}

	/**
	 * @return schematron test phase name
	 */
	public String getTest() {
		return test;
	}

	/**
	 * @param schematron test phase name
	 */
	public void setTest(String test) {
		this.test = test;
	}

	/**
	 * @return schematron role infomation
	 */
	public Role getRole() {
		return role;
	}

	/**
	 * @param schematron role infomation
	 */
	public void setRole(Role role) {
		this.role = role;
	}
}
