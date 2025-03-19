import React, { useState, useEffect } from "react";
import { gsap } from "gsap";
import apiLink from "../services/apiLink";
import "./CashFlowPanel.css";

function CashFlowPanel({ token }) {
  const [flows, setFlows] = useState([]);
  const [desc, setDesc] = useState("");
  const [amt, setAmt] = useState("");
  const [dt, setDt] = useState("");
  const [catId, setCatId] = useState("");
  const [payId, setPayId] = useState("");
  const [flowType, setFlowType] = useState("income");

  const [cats, setCats] = useState([]);
  const [pays, setPays] = useState([]);
  const [sortField, setSortField] = useState("flowDate");
  const [sortOrder, setSortOrder] = useState("desc");

  const loadFlows = async () => {
    try {
      const data = await apiLink.getFlows(token);
      setFlows(data);
      gsap.fromTo(
        ".cashflow-table tbody tr",
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, stagger: 0.1, duration: 0.5 }
      );
    } catch (error) {
      console.error("Error loading transactions: ", error);
    }
  };

  useEffect(() => {
    loadFlows();
    apiLink.getCategories(token).then(setCats);
    apiLink.getPayWays(token).then(setPays);
  }, [token]);

  const handleAddFlow = async () => {
    if (!desc || !amt || !dt) {
      alert("All fields must be filled!");
      return;
    }

    const newFlow = {
      FlowDesc: desc,
      FlowAmount:
        flowType === "expense"
          ? -Math.abs(parseFloat(amt))
          : Math.abs(parseFloat(amt)),
      FlowDate: new Date(dt).toISOString(),
      CatId: catId || null,
      PayId: payId || null,
      FlowType: flowType,
    };

    try {
      await apiLink.addFlow(token, newFlow);
      loadFlows();
    } catch (err) {
      console.error("Error adding transaction: ", err);
    }

    setDesc("");
    setAmt("");
    setDt("");
    setCatId("");
    setPayId("");
  };

  const removeFlow = async (id) => {
    if (!window.confirm("Delete this transaction?")) return;

    try {
      await apiLink.delFlow(token, id);
      loadFlows();
    } catch (err) {
      console.error("Error deleting transaction: ", err);
    }
  };

  const sortData = (field) => {
    const order = sortField === field && sortOrder === "asc" ? "desc" : "asc";
    setSortField(field);
    setSortOrder(order);

    const sorted = [...flows].sort((a, b) => {
      let valA, valB;

      if (field === "categoryName" || field === "payLabel") {
        valA = a[field] || "";
        valB = b[field] || "";
      } else {
        valA = a[field]?.toString() || "";
        valB = b[field]?.toString() || "";
      }

      return order === "asc"
        ? valA.localeCompare(valB, undefined, { numeric: true })
        : valB.localeCompare(valA, undefined, { numeric: true });
    });

    setFlows(sorted);
  };

  return (
    <div className="cashflow-container">
      <h2>📊 Cash Flows</h2>
      <div className="cashflow-form">
        <input
          placeholder="Desc"
          value={desc}
          onChange={(e) => setDesc(e.target.value)}
        />
        <input
          placeholder="Amount"
          type="number"
          value={amt}
          onChange={(e) => setAmt(e.target.value)}
        />
        <input
          placeholder="Date"
          type="date"
          value={dt}
          onChange={(e) => setDt(e.target.value)}
        />

        <select value={flowType} onChange={(e) => setFlowType(e.target.value)}>
          <option value="income">Income</option>
          <option value="expense">Expense</option>
        </select>

        <select value={catId} onChange={(e) => setCatId(e.target.value)}>
          <option value="">(no category)</option>
          {cats.map((c) => (
            <option key={c.id} value={c.id}>
              {c.catName}
            </option>
          ))}
        </select>

        <select value={payId} onChange={(e) => setPayId(e.target.value)}>
          <option value="">(no payway)</option>
          {pays.map((p) => (
            <option key={p.id} value={p.id}>
              {p.payLabel}
            </option>
          ))}
        </select>

        <button onClick={handleAddFlow}>Add</button>
      </div>

      <div className="cashflow-table-container">
        <table className="cashflow-table">
          <thead>
            <tr>
              <th onClick={() => sortData("flowDesc")}>Desc ⬍</th>
              <th onClick={() => sortData("flowAmount")}>Amount ⬍</th>
              <th onClick={() => sortData("flowDate")}>Date ⬍</th>
              <th onClick={() => sortData("flowType")}>Type ⬍</th>
              <th onClick={() => sortData("categoryName")}>Category ⬍</th>
              <th onClick={() => sortData("payLabel")}>Payway ⬍</th>
              <th>Del</th>
            </tr>
          </thead>
          <tbody>
            {flows.map((f) => (
              <tr key={f.id}>
                <td>{f.flowDesc}</td>
                <td>{f.flowAmount}</td>
                <td>{new Date(f.flowDate).toLocaleDateString()}</td>
                <td>{f.flowType}</td>
                <td>{f.categoryName || "—"}</td>
                <td>{f.payLabel || "—"}</td>
                <td>
                  <button onClick={() => removeFlow(f.id)}>X</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default CashFlowPanel;
