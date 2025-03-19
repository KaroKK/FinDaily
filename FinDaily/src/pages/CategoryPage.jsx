import React, { useState, useEffect } from "react";
import { gsap } from "gsap";
import apiLink from "../services/apiLink";
import "./CategoryPage.css";

function CategoryPage({ token }) {
  const [categories, setCategories] = useState([]);
  const [catName, setCatName] = useState("");
  const [catInfo, setCatInfo] = useState("");
  const [sortField, setSortField] = useState("catName");
  const [sortOrder, setSortOrder] = useState("asc");

  const loadCategories = async () => {
    try {
      const data = await apiLink.getCategories(token);
      setCategories(data);
      gsap.fromTo(
        ".category-table tbody tr",
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, stagger: 0.1, duration: 0.5 }
      );
    } catch (err) {
      console.error("Error loading categories:", err);
    }
  };

  useEffect(() => {
    loadCategories();
  }, [token]);

  const addCategory = async () => {
    try {
      await apiLink.addCategory(token, { catName, catInfo });
      setCatName("");
      setCatInfo("");
      loadCategories();
    } catch (err) {
      alert("Error: " + (err.response?.data || err.message));
    }
  };

  const removeCategory = async (id) => {
    if (!window.confirm("Delete category?")) return;
    try {
      await apiLink.delCategory(token, id);
      loadCategories();
    } catch (err) {
      alert("Error: " + (err.response?.data || err.message));
    }
  };

  const sortData = (field) => {
    const order = sortField === field && sortOrder === "asc" ? "desc" : "asc";
    setSortField(field);
    setSortOrder(order);
    const sorted = [...categories].sort((a, b) =>
      order === "asc"
        ? a[field].localeCompare(b[field])
        : b[field].localeCompare(a[field])
    );
    setCategories(sorted);
  };

  return (
    <div className="category-container">
      <h2>📂 Categories</h2>
      <div className="category-form">
        <input
          placeholder="Category Name"
          value={catName}
          onChange={(e) => setCatName(e.target.value)}
        />
        <input
          placeholder="Category Info"
          value={catInfo}
          onChange={(e) => setCatInfo(e.target.value)}
        />
        <button onClick={addCategory}>Add</button>
      </div>

      <div className="category-table-container">
        <table className="category-table">
          <thead>
            <tr>
              <th onClick={() => sortData("catName")}>Name ⬍</th>
              <th onClick={() => sortData("catInfo")}>Info ⬍</th>
              <th>Del</th>
            </tr>
          </thead>
          <tbody>
            {categories.map((c) => (
              <tr key={c.id}>
                <td>{c.catName}</td>
                <td>{c.catInfo}</td>
                <td>
                  <button onClick={() => removeCategory(c.id)}>X</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default CategoryPage;
