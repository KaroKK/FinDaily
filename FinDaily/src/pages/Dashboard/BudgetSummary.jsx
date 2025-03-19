import React from "react";
import "./BudgetSummary.css";

function BudgetSummary({ budgets }) {
  return (
    <div className="budget-summary">
      {budgets.length === 0 ? (
        <h5>No set budget for this month.</h5>
      ) : (
        budgets.map((budget) => {
          const spent = budget.spent ?? 0;
          const limitAmt = budget.limitAmt ?? 1; //protect from 0 devid.
          const percentage = (spent / limitAmt) * 100;
          const adjustedPercentage = Math.min(percentage, 100);

          // dynam. color stripe
          const progressColor =
            percentage >= 100
              ? "#f44336" // red, budget exceeded
              : percentage >= 75
              ? "#ff9800" // orange (more,but withing budget)
              : "#4caf50"; // green (within budget, safe)

          return (
            <div key={budget.id} className="budget-item">
              <div className="budget-header">
                <span className="budget-category">{budget.categoryName}</span>
                <span className="budget-amount">
                  {spent.toFixed(2)} / {limitAmt.toFixed(2)}
                </span>
              </div>
              <div className="progress-bar">
                <div
                  className="progress-fill"
                  style={{
                    width: `${adjustedPercentage}%`,
                    background: `linear-gradient(90deg, #4caf50 0%, #ff9800 75%, #f44336 100%)`,
                  }}
                ></div>
              </div>
              <p
                className={`budget-status ${
                  percentage >= 100 ? "over-budget-text" : ""
                }`}
              >
                {percentage >= 100
                  ? "⚠ Budget exceeded!"
                  : percentage >= 75
                  ? "⚠ Nearing budget limit"
                  : "✅ Within budget"}
              </p>
            </div>
          );
        })
      )}
    </div>
  );
}

export default BudgetSummary;
