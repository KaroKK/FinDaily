import React, { useRef, useEffect } from "react";
import { gsap } from "gsap";
import { Link } from "react-router-dom";
import "./SideMenu.css";

function SideMenu() {
  const menuRef = useRef(null);

  //GSAP
  useEffect(() => {
    gsap.fromTo(
      menuRef.current,
      { x: -250, opacity: 0 },
      { x: 0, opacity: 1, duration: 1, ease: "power3.out" }
    );
  }, []);

  return (
    <div className="side-menu" ref={menuRef}>
      <nav>
        <Link to="/dashboard">🏠 Dashboard</Link>
        <Link to="/cashflows">💰 Cash Flows</Link>
        <Link to="/categories">📂 Categories</Link>
        <Link to="/payways">💳 Payment Methods</Link>
        <Link to="/budgets">📊 Budgets</Link>
      </nav>
    </div>
  );
}

export default SideMenu;
