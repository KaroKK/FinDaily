import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import apiLink from "./services/apiLink";
import HomeLayout from "./pages/HomeLayout";
import "./App.css";

function App() {
  const [token, setToken] = useState("");
  const [isRegister, setIsRegister] = useState(false);
  const [loginUser, setLoginUser] = useState("");
  const [loginPass, setLoginPass] = useState("");
  const [regUser, setRegUser] = useState("");
  const [regPass, setRegPass] = useState("");

  const navigate = useNavigate();

  useEffect(() => {
    const savedToken = sessionStorage.getItem("finToken");
    if (savedToken) {
      setToken(savedToken);
    }
  }, []);

  const doLogin = async () => {
    try {
      const res = await apiLink.loginUser(loginUser, loginPass);
      setToken(res.token);
      sessionStorage.setItem("finToken", res.token);

      setLoginUser("");
      setLoginPass("");

      // save user in sessionstorage
      sessionStorage.setItem("user", JSON.stringify({ UserName: loginUser }));

      console.log(
        "User saved to sessionStorage:",
        sessionStorage.getItem("user")
      );
      navigate("/dashboard"); // to dashboard
    } catch (err) {
      alert("Login error: " + (err.response?.data || err.message));

      // clear input
      setLoginUser("");
      setLoginPass("");
    }
  };

  const doRegister = async () => {
    try {
      await apiLink.registerUser(regUser, regPass);
      alert("Registered successfully. Now log in.");

      // clear input
      setRegUser("");
      setRegPass("");

      setIsRegister(false);
    } catch (err) {
      alert("Register error: " + (err.response?.data || err.message));

      // clear input
      setRegUser("");
      setRegPass("");
    }
  };

  const doLogout = () => {
    sessionStorage.removeItem("finToken");
    sessionStorage.removeItem("user");
    setToken("");
    navigate("/");
  };

  // Enter key in Form
  const handleKeyDown = (event) => {
    if (event.key === "Enter") {
      event.preventDefault();
      isRegister ? doRegister() : doLogin();
    }
  };

  if (!token) {
    return (
      <div className="auth-container">
        <div className="auth-box">
          <h1>{isRegister ? "Register" : "Login"}</h1>

          <form onKeyDown={handleKeyDown}>
            <input
              className="input"
              placeholder="Username"
              value={isRegister ? regUser : loginUser}
              onChange={(e) =>
                isRegister
                  ? setRegUser(e.target.value)
                  : setLoginUser(e.target.value)
              }
            />

            <input
              className="input"
              placeholder="Password"
              type="password"
              value={isRegister ? regPass : loginPass}
              onChange={(e) =>
                isRegister
                  ? setRegPass(e.target.value)
                  : setLoginPass(e.target.value)
              }
            />

            <button
              className="btn"
              type="button"
              onClick={isRegister ? doRegister : doLogin}
            >
              {isRegister ? "Sign Up" : "Log in"}
            </button>
          </form>

          <p>
            {isRegister ? "Already have an account?" : "No account?"}{" "}
            <span className="link" onClick={() => setIsRegister(!isRegister)}>
              {isRegister ? "Log in" : "Sign up"}
            </span>
          </p>
        </div>
      </div>
    );
  }

  return <HomeLayout token={token} setToken={setToken} doLogout={doLogout} />;
}

export default App;
