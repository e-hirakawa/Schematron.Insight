package jp.gr.java_conf.schkit;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import jp.gr.java_conf.schkit.enums.ResultStatus;

public class ResultCollection implements Iterable<Result> {

	private ArrayList<Result> items;

	public ResultCollection() {
		items = new ArrayList<Result>();
	}

	public Result get(int index) {
		if (index >= 0 && index < items.size())
			return items.get(index);
		return null;
	}

	public void set(int index, Result item) {
		if (index >= 0 && index < items.size())
			items.set(index, item);
	}

	public void add(Result item) {
		items.add(item);
	}

	public void addRange(List<Result> results) {
		items.addAll(results);
	}

	public void remove(Result result) {
		items.remove(result);
	}

	public void remove(int index) {
		if (index >= 0 && index < items.size())
			items.remove(index);
	}

	public int size() {
		return items.size();
	}

	public int sizeOfSyntaxError() {
		return sizeOf(ResultStatus.SyntaxError);
	}

	public int sizeOfAssert() {
		return sizeOf(ResultStatus.Assert);
	}

	public int sizeOfReport() {
		return sizeOf(ResultStatus.Report);
	}

	private int sizeOf(ResultStatus status) {
		int count = 0;
		for (Result item : items) {
			if (item.getStatus() == status)
				count++;
		}
		return count;
	}

	public boolean isValid() {
		return size() == 0;
	}

	@Override
	public Iterator<Result> iterator() {
		// TODO 自動生成されたメソッド・スタブ
		return items.iterator();
	}

}
