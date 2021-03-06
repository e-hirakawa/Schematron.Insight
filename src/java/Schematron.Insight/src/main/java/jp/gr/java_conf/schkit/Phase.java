package jp.gr.java_conf.schkit;

import java.util.ArrayList;
import java.util.List;

import org.apache.commons.lang3.StringUtils;

public class Phase {
	public static String ALL = "#ALL";
	public static String DEFAULT = "#DEFAULT";
	private String id="";
	private List<String> patterns = new ArrayList<String>();

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public List<String> getPatterns() {
		return patterns;
	}

	public void setPatterns(List<String> patterns) {
		this.patterns = patterns;
	}

	public boolean isEnabled() {
		return !StringUtils.isBlank(id) && patterns.size() > 0;
	}

	public Phase() {
		id = "";
		patterns = new ArrayList<String>();
	}

}
