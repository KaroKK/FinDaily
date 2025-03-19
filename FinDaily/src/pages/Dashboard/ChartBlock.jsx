import React from "react";
import { Bar, Line, PolarArea } from "react-chartjs-2";
import "../utils/ChartSetup";
import "./ChartBlock.css";

const predefinedColors = [
  "#1F9595",
  "#3B59AB",
  "#F3C13A",
  "#D24D57",
  "#6C7A89",
  "#F9690E",
];

function ChartBlock({
  title,
  chartType,
  labels = [],
  values = [],
  datasets = [],
}) {
  let data;

  if (datasets.length > 0) {
    // many datasets (f.e. comparing categories)
    data = {
      labels,
      datasets: datasets.map((dataset, index) => ({
        label: dataset.label,
        data: dataset.data,
        backgroundColor: predefinedColors[index % predefinedColors.length],
        borderColor: predefinedColors[index % predefinedColors.length],
        borderWidth: 2,
      })),
    };
  } else {
    // one dataset
    const validValues = values.map((val) =>
      typeof val === "number" ? val : 0
    );
    const validLabels = labels.filter((_, index) => validValues[index] !== 0);

    if (!validLabels.length || !validValues.length) {
      return <p>📉 No data available for {title}...</p>;
    }

    data = {
      labels: validLabels,
      datasets: [
        {
          label: title,
          data: validValues,
          backgroundColor: predefinedColors.slice(0, validValues.length),
          borderColor: predefinedColors.slice(0, validValues.length),
          borderWidth: 2,
        },
      ],
    };
  }

  // charts
  return (
    <div className="chart-block">
      <h3>{title}</h3>
      {chartType === "bar" && <Bar data={data} />}
      {chartType === "polarArea" && <PolarArea data={data} />}
      {chartType === "line" && <Line data={data} />}
    </div>
  );
}

export default ChartBlock;
