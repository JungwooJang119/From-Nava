using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportTextBank : MonoBehaviour
{
	[Serializable]
	private struct ReportInfo {
		public string name;
		public TextContainer data;
	}
	[SerializeField] private ReportInfo[] reports;

	private Dictionary<string, TextContainer> reportDict;

	// Initialize Polaroid Dictionary;
	void Start() {
		reportDict = new Dictionary<string, TextContainer>();
		foreach (ReportInfo report in reports) {
			reportDict[report.name] = report.data;
		}
	}

	public TextContainer GetReportData(string name) {
		return reportDict[name];
	}
}
