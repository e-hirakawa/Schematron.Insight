package jp.gr.java_conf.schkit;

public class RoleColor {
	private int r = -1;
	private int g = -1;
	private int b = -1;
	private int a = 255;

	public int getR() {
		return r;
	}

	public void setR(int r) {
		this.r = r;
	}

	public int getG() {
		return g;
	}

	public void setG(int g) {
		this.g = g;
	}

	public int getB() {
		return b;
	}

	public void setB(int b) {
		this.b = b;
	}

	public int getA() {
		return a;
	}

	public void setA(int a) {
		this.a = a;
	}

	public RoleColor(int r, int g, int b, int a) {
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public RoleColor(int r, int g, int b) {
		this.r = r;
		this.g = g;
		this.b = b;
	}

	public RoleColor() {

	}

	public boolean isEnabled() {
		return r >= 0 && g >= 0 && b >= 0;
	}
}