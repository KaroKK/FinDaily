import React, { useState } from "react";
import "./StatsFilter.css";

function StatsFilter({ onFilterChange }) {
  const [fromDate, setFromDate] = useState(() => {
    const now = new Date();
    now.setUTCHours(0, 0, 0, 0); // reset to UTC
    return new Date(Date.UTC(now.getFullYear(), now.getMonth(), 1))
      .toISOString()
      .split("T")[0]; // first day of the month in UTC
  });

  const [toDate, setToDate] = useState(
    () => new Date().toISOString().split("T")[0]
  );

  const applyFilter = () => {
    const params = { from: fromDate, to: toDate };
    onFilterChange(params);
  };

  return (
    <div className="stats-filter">
      <h3>🔍 Filters</h3>
      <input
        type="date"
        value={fromDate}
        onChange={(e) => setFromDate(e.target.value)}
      />
      <input
        type="date"
        value={toDate}
        onChange={(e) => setToDate(e.target.value)}
      />
      <button className="apply-btn" onClick={applyFilter}>
        Apply
      </button>
    </div>
  );
}

export default StatsFilter;
