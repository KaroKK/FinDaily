import React, { useState, useEffect } from "react";
import { gsap } from "gsap";
import apiLink from "../services/apiLink";
import "./PayWayPage.css";

function PayWayPage({ token }) {
  const [pays, setPays] = useState([]);
  const [payLabel, setPayLabel] = useState("");
  const [payInfo, setPayInfo] = useState("");
  const [sortField, setSortField] = useState("payLabel");
  const [sortOrder, setSortOrder] = useState("asc");

  const loadPays = async () => {
    try {
      const data = await apiLink.getPayWays(token);
      setPays(data);

      //GSAP starts after data loading
      setTimeout(() => {
        gsap.fromTo(
          ".payway-table tbody tr",
          { opacity: 0, y: 10 },
          { opacity: 1, y: 0, stagger: 0.1, duration: 0.5 }
        );
      }, 100);
    } catch (err) {
      console.error("Error loading payment methods:", err);
    }
  };

  useEffect(() => {
    if (token) {
      loadPays();
    }
  }, [token]);

  const addPayWay = async () => {
    try {
      await apiLink.addPayWay(token, { payLabel, payInfo });
      setPayLabel("");
      setPayInfo("");
      loadPays();
    } catch (err) {
      alert("Error: " + (err.response?.data || err.message));
    }
  };

  const removePayWay = async (id) => {
    if (!window.confirm("Delete payment method?")) return;
    try {
      await apiLink.delPayWay(token, id);
      loadPays();
    } catch (err) {
      alert("Error: " + (err.response?.data || err.message));
    }
  };

  const sortData = (field) => {
    const order = sortField === field && sortOrder === "asc" ? "desc" : "asc";
    setSortField(field);
    setSortOrder(order);

    const sorted = [...pays].sort((a, b) =>
      order === "asc"
        ? (a[field] || "").localeCompare(b[field] || "")
        : (b[field] || "").localeCompare(a[field] || "")
    );

    setPays(sorted);
  };

  return (
    <div className="payway-container">
      <h2>💳 Payment Methods</h2>
      <form
        className="payway-form"
        onKeyDown={(e) => e.key === "Enter" && addPayWay()}
      >
        <input
          placeholder="Payment Label"
          value={payLabel}
          onChange={(e) => setPayLabel(e.target.value)}
        />
        <input
          placeholder="Payment Info"
          value={payInfo}
          onChange={(e) => setPayInfo(e.target.value)}
        />
        <button type="button" onClick={addPayWay}>
          Add
        </button>
      </form>

      <div className="payway-table-container">
        <table className="payway-table">
          <thead>
            <tr>
              <th onClick={() => sortData("payLabel")}>Label ⬍</th>
              <th onClick={() => sortData("payInfo")}>Info ⬍</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {pays.length > 0 ? (
              pays.map((p) => (
                <tr key={p.id}>
                  <td>{p.payLabel}</td>
                  <td>{p.payInfo || "No info"}</td>
                  <td>
                    <button onClick={() => removePayWay(p.id)}>X</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="3">No payment methods found.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default PayWayPage;
