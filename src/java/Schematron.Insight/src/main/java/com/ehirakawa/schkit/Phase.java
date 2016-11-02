package com.ehirakawa.schkit;

import java.util.ArrayList;

import org.apache.commons.lang3.StringUtils;

public class Phase {
	public static final String ALL = "#ALL";
	public static final String DEFAULT = "#DEFAULT";

	private String id = "";
	private ArrayList<String> patterns = new ArrayList<String>();

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public ArrayList<String> getPatterns() {
		return patterns;
	}

	public void setPatterns(ArrayList<String> patterns) {
		this.patterns = patterns;
	}

	public boolean isEnabled() {
		return !StringUtils.isBlank(id) && patterns.size() > 0;
	}
}
