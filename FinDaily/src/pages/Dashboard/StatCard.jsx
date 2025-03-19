import React from "react";
import "./StatCard.css";

const StatCard = ({ title, value, color }) => {
  return (
    <div className="stat-card" style={{ backgroundColor: color }}>
      <h4>{title}</h4>
      <p>{parseFloat(value).toFixed(2)}</p>{" "}
    </div>
  );
};

export default StatCard;
