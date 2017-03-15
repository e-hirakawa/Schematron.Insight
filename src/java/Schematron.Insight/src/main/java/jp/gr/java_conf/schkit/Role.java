package jp.gr.java_conf.schkit;

import org.apache.commons.lang3.StringUtils;

public class Role {

    private String name = "";
    public RoleColor foreColor = new RoleColor();
    public RoleColor backColor = new RoleColor();

    public Role(String name) {
        this.name = name;
    }

    public Role() {
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public RoleColor getForeColor() {
        return foreColor;
    }

    public void setForeColor(RoleColor foreColor) {
        this.foreColor = foreColor;
    }

    public RoleColor getBackColor() {
        return backColor;
    }

    public void setBackColor(RoleColor backColor) {
        this.backColor = backColor;
    }

    public boolean isEnabled() {
        return !StringUtils.isBlank(name);
    }
}
