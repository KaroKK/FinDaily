import React, { useState, useEffect, useRef } from "react";
import { gsap } from "gsap";
import { FiLogOut } from "react-icons/fi";
import { FaUserCircle } from "react-icons/fa";
import "./TopBar.css";

function TopBar({ doLogout }) {
  const barRef = useRef(null);
  const btnRef = useRef(null);

  // get username from sessionStorage
  const [userName, setUserName] = useState(() => {
    try {
      const storedUser = sessionStorage.getItem("user");
      return storedUser ? JSON.parse(storedUser).UserName : "Guest";
    } catch {
      return "Guest";
    }
  });

  useEffect(() => {
    gsap.fromTo(
      barRef.current,
      { y: -80, opacity: 0 },
      { y: 0, opacity: 1, duration: 1, ease: "power2.out" }
    );

    gsap.fromTo(
      btnRef.current,
      { scale: 0.8, opacity: 0 },
      {
        scale: 1,
        opacity: 1,
        duration: 0.6,
        delay: 0.5,
        ease: "elastic.out(1, 0.5)",
      }
    );
  }, []);

  return (
    <header ref={barRef} className="top-bar">
      <div className="logo">📊 FinDaily - Manage Your Budget</div>
      <div className="user-info">
        <FaUserCircle className="user-icon" />
        <span className="user-name">Hello, {userName}! 👋</span>
        <button ref={btnRef} className="logout-btn" onClick={doLogout}>
          <FiLogOut className="logout-icon" />
          Logout
        </button>
      </div>
    </header>
  );
}

export default TopBar;
