import React, { useState, useEffect, useRef } from "react";
import ChartArea from "../ChartArea";
import { gsap } from "gsap";
import apiLink from "../../services/apiLink";
import StatsFilter from "./StatsFilter";
import StatCard from "./StatCard";
import ChartBlock from "./ChartBlock";
import BudgetSummary from "./BudgetSummary";
import "./Dashboard.css";

function Dashboard({ token }) {
  const today = new Date();
  const firstDayOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
  firstDayOfMonth.setHours(12);

  const [filterParams, setFilterParams] = useState({
    from: firstDayOfMonth.toISOString().split("T")[0],
    to: today.toISOString().split("T")[0],
  });

  const [budgetMonth, setBudgetMonth] = useState(
    new Date().toISOString().slice(0, 7)
  );
  const containerRef = useRef(null);
  const [stats, setStats] = useState(null);
  const [budgets, setBudgets] = useState([]);

  useEffect(() => {
    if (containerRef.current) {
      gsap.fromTo(
        containerRef.current,
        { y: 50, opacity: 0 },
        { y: 0, opacity: 1, duration: 0.8, ease: "power2.out" }
      );
    }
  }, []);

  const fetchStats = async () => {
    if (!token) return;

    try {
      const data = await apiLink.getStats(token, filterParams);
      setStats({
        summary: data.summary || {
          totalIncome: 0,
          totalExpense: 0,
          balance: 0,
        },
        monthlyFlow: data.monthlyFlow || { labels: [], values: [] },
        categoryDist: data.categoryStats
          ? {
              labels: data.categoryStats.map(
                (c) => c.categoryName || "Unknown"
              ),
              values: data.categoryStats.map(
                (c) => Math.abs(c.totalAmount) || 0
              ),
            }
          : { labels: [], values: [] },
        dailyTrend: {
          labels: data.dailyTrend?.labels || [],
          values: data.dailyTrend?.values || [],
        },
        monthlyComparison:
          data.monthlyComparison?.map((c) => ({
            Month: new Date(`${c.year}-${c.month}-01`).toLocaleDateString(
              "en-GB",
              {
                year: "numeric",
                month: "long",
              }
            ),
            Income: c.income ?? 0,
            Expense: Math.abs(c.expense ?? 0),
          })) || [],
      });
    } catch (error) {
      console.error("Error fetching stats:", error);
      setStats(null);
    }
  };

  const fetchBudgets = async () => {
    if (!token) return;

    try {
      // get budgets and categ.
      const budgetsData = await apiLink.getBudgets(token);
      const categoriesData = await apiLink.getCategories(token);

      // cashflows this month
      const startOfMonth = `${budgetMonth}-01T00:00:00.000Z`;
      const endOfMonth =
        new Date(
          new Date(budgetMonth).getFullYear(),
          new Date(budgetMonth).getMonth() + 1,
          0
        )
          .toISOString()
          .split("T")[0] + "T23:59:59.999Z";

      const expensesData = await apiLink.getStats(token, {
        from: startOfMonth,
        to: endOfMonth,
      });

      const categoryNameToId = {};
      categoriesData.forEach((category) => {
        categoryNameToId[category.catName] = category.id;
      });

      const categoryExpenses = {};
      if (expensesData && expensesData.categoryStats) {
        expensesData.categoryStats.forEach((expense) => {
          const catId = categoryNameToId[expense.categoryName];
          if (catId) {
            categoryExpenses[catId] = Math.abs(expense.totalAmount) || 0;
          }
        });
      }

      // cashflows to budgets
      const filteredBudgets = budgetsData
        .filter((budget) => budget.periodTxt === budgetMonth) // monthly filter
        .map((budget) => ({
          ...budget,
          categoryName:
            categoriesData.find((c) => c.id === budget.catId)?.catName ||
            "No category assigned",
          spent: categoryExpenses[budget.catId] || 0,
        }));

      setBudgets(filteredBudgets);
    } catch (error) {
      console.error("Error fetching budgets:", error);
      setBudgets([]);
    }
  };

  useEffect(() => {
    fetchStats();
  }, [filterParams, token]);

  useEffect(() => {
    fetchBudgets();
  }, [budgetMonth, token]);

  const handleFilterChange = (params) => {
    setFilterParams((prev) => ({
      ...prev,
      ...params,
    }));
  };

  return (
    <div className="dashboard" ref={containerRef}>
      <p className="dashboard-title"></p>
      <div className="dashboard-layout">
        <div className="dashboard-side">
          <StatsFilter token={token} onFilterChange={handleFilterChange} />
          <div className="filter">
            <h3>📊 Using the set budget</h3>

            <select
              value={budgetMonth}
              onChange={(e) => setBudgetMonth(e.target.value)}
            >
              {Array.from({ length: 12 }, (_, i) => {
                const date = new Date();
                date.setMonth(date.getMonth() - i);
                const monthValue = date.toISOString().slice(0, 7);
                return (
                  <option key={i} value={monthValue}>
                    {date.toLocaleDateString("en-GB", {
                      year: "numeric",
                      month: "long",
                    })}
                  </option>
                );
              })}
            </select>
          </div>
          <BudgetSummary budgets={budgets} />
        </div>
        <div className="dashboard-main">
          {stats ? (
            <>
              <div className="stats-row">
                <StatCard
                  title="💰 Total Income"
                  value={stats.summary.totalIncome.toFixed(2)}
                  color="#4caf50"
                />
                <StatCard
                  title="💸 Total Expense"
                  value={stats.summary.totalExpense.toFixed(2)}
                  color="#f44336"
                />
                <StatCard
                  title="📈 Balance"
                  value={stats.summary.balance.toFixed(2)}
                  color="#2196f3"
                />
              </div>
              <div className="charts-row">
                <ChartBlock
                  title="📊 Monthly Savings"
                  chartType="bar"
                  labels={stats.monthlyFlow.labels}
                  values={stats.monthlyFlow.values}
                  background="#4caf50"
                />
                {stats.categoryDist.labels.length > 0 ? (
                  <ChartBlock
                    title="📌 Category Distribution"
                    chartType="polarArea"
                    labels={stats.categoryDist.labels}
                    values={stats.categoryDist.values}
                    background={["#ff5722", "#03a9f4", "#8bc34a", "#ff9800"]}
                  />
                ) : (
                  <p>📉 No data available for 📌 Category Distribution</p>
                )}
              </div>
              <div className="charts-row">
                <ChartBlock
                  title="📅 Daily Trend"
                  chartType="line"
                  labels={stats.dailyTrend.labels}
                  values={stats.dailyTrend.values}
                  background="#2196f3"
                />
              </div>
              {stats.monthlyComparison.length > 0 ? (
                <ChartBlock
                  title="📊 Monthly Income vs Expense"
                  chartType="bar"
                  labels={stats.monthlyComparison.map((c) => c.Month)}
                  datasets={[
                    {
                      label: "Income",
                      data: stats.monthlyComparison.map((c) => c.Income ?? 0),
                      backgroundColor: "rgba(76, 175, 80, 0.6)",
                    },
                    {
                      label: "Expense",
                      data: stats.monthlyComparison.map((c) => c.Expense ?? 0),
                      backgroundColor: "rgba(244, 67, 54, 0.6)",
                    },
                  ]}
                />
              ) : (
                <p>📉 No data available for Monthly Comparison</p>
              )}
              <ChartArea token={token} filterParams={filterParams} />
            </>
          ) : (
            <p>⏳ Loading...</p>
          )}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
