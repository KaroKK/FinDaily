import React, { useEffect } from "react";
import { Routes, Route, Navigate, useNavigate } from "react-router-dom";
import SideMenu from "../layout/SideMenu";
import TopBar from "../layout/Top_Bar";
import Dashboard from "./Dashboard/Dashboard";
import CashFlowPanel from "../pages/CashFlowPanel";
import CategoryPage from "../pages/CategoryPage";
import PayWayPage from "../pages/PayWayPage";
import BudgetPage from "../pages/BudgetPage";
import Footer from "../layout/Footer";
import "./HomeLayout.css";

function HomeLayout({ token, setToken, doLogout }) {
  const navigate = useNavigate();

  useEffect(() => {
    const savedToken = sessionStorage.getItem("finToken");
    if (!savedToken) {
      setToken("");
      navigate("/");
    }
  }, [token, navigate]);

  return (
    <div className="home-layout">
      {token ? (
        <>
          <SideMenu doLogout={doLogout} />
          <div className="main-area">
            <TopBar doLogout={doLogout} />
            <div className="content-area">
              <Routes>
                <Route path="/" element={<Navigate to="/dashboard" />} />
                <Route
                  path="/dashboard"
                  element={<Dashboard token={token} />}
                />
                <Route
                  path="/cashflows"
                  element={<CashFlowPanel token={token} />}
                />
                <Route
                  path="/categories"
                  element={<CategoryPage token={token} />}
                />
                <Route path="/payways" element={<PayWayPage token={token} />} />
                <Route path="/budgets" element={<BudgetPage token={token} />} />
                <Route path="*" element={<h2>Not found</h2>} />
              </Routes>
            </div>
            <Footer /> {}
          </div>
        </>
      ) : (
        <Navigate to="/" />
      )}
    </div>
  );
}

export default HomeLayout;
