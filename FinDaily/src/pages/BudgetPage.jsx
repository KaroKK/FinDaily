import React, { useState, useEffect } from "react";
import { gsap } from "gsap";
import apiLink from "../services/apiLink";
import "./BudgetPage.css";

function BudgetPage({ token }) {
  const [budgets, setBudgets] = useState([]);
  const [catId, setCatId] = useState("");
  const [limitAmt, setLimitAmt] = useState("");
  const [periodTxt, setPeriodTxt] = useState("");
  const [cats, setCats] = useState([]);
  const [sortField, setSortField] = useState("periodTxt");
  const [sortOrder, setSortOrder] = useState("asc");

  const loadBudgets = async () => {
    try {
      const data = await apiLink.getBudgets(token);
      setBudgets(data);
      gsap.fromTo(
        ".budget-table tbody tr",
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, stagger: 0.1, duration: 0.5 }
      );
    } catch (error) {
      console.error("Error loading budgets:", error);
    }
  };

  const loadCats = async () => {
    try {
      const data = await apiLink.getCategories(token);
      setCats(data);
    } catch (error) {
      console.error("Error loading categories:", error);
    }
  };

  useEffect(() => {
    loadBudgets();
    loadCats();
  }, [token]);

  const addBudget = async () => {
    if (!catId || !limitAmt || !periodTxt) {
      alert("All fields must be filled!");
      return;
    }

    try {
      await apiLink.addBudget(token, {
        catId: parseInt(catId),
        limitAmt: parseFloat(limitAmt),
        periodTxt,
      });
      setCatId("");
      setLimitAmt("");
      setPeriodTxt("");
      loadBudgets();
    } catch (err) {
      alert("Error: " + (err.response?.data || err.message));
    }
  };

  const removeBudget = async (id) => {
    if (!window.confirm("Delete budget?")) return;

    try {
      await apiLink.delBudget(token, id);
      loadBudgets();
    } catch (err) {
      alert("Error: " + (err.response?.data || err.message));
    }
  };

  const sortData = (field) => {
    const order = sortField === field && sortOrder === "asc" ? "desc" : "asc";
    setSortField(field);
    setSortOrder(order);

    const sorted = [...budgets].sort((a, b) => {
      let valA, valB;

      if (field === "categoryName") {
        valA = a.theCat?.catName || "";
        valB = b.theCat?.catName || "";
      } else {
        valA = a[field]?.toString() || "";
        valB = b[field]?.toString() || "";
      }

      return order === "asc"
        ? valA.localeCompare(valB, undefined, { numeric: true })
        : valB.localeCompare(valA, undefined, { numeric: true });
    });

    setBudgets(sorted);
  };

  return (
    <div className="budget-container">
      <h2>📊 Budgets</h2>
      <div className="budget-form">
        <select value={catId} onChange={(e) => setCatId(e.target.value)}>
          <option value="">(select category)</option>
          {cats.map((c) => (
            <option key={c.id} value={c.id}>
              {c.catName}
            </option>
          ))}
        </select>
        <input
          placeholder="Limit Amount"
          type="number"
          value={limitAmt}
          onChange={(e) => setLimitAmt(e.target.value)}
        />
        <input
          placeholder="Period (e.g. 2025-03)"
          value={periodTxt}
          onChange={(e) => setPeriodTxt(e.target.value)}
        />
        <button onClick={addBudget}>Add</button>
      </div>

      <div className="budget-table-container">
        <table className="budget-table">
          <thead>
            <tr>
              <th onClick={() => sortData("categoryName")}>Category ⬍</th>
              <th onClick={() => sortData("limitAmt")}>Limit ⬍</th>
              <th onClick={() => sortData("periodTxt")}>Period ⬍</th>
              <th>Del</th>
            </tr>
          </thead>
          <tbody>
            {budgets.length > 0 ? (
              budgets.map((b) => (
                <tr key={b.id}>
                  <td>{b.theCat?.catName || "—"}</td>
                  <td>{b.limitAmt}</td>
                  <td>{b.periodTxt}</td>
                  <td>
                    <button onClick={() => removeBudget(b.id)}>X</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="4">No budgets found.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default BudgetPage;
