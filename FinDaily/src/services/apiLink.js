import axios from "axios";

const BASE = import.meta.env.VITE_API_URL || "http://localhost:8080/api";
console.log("Base URL:", BASE);

const api = axios.create({
  baseURL: BASE,
  headers: { "Content-Type": "application/json" },
});



// autom. setting token in headers
const setAuthToken = (token) => {
  if (token) {
    api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common["Authorization"];
  }
};

// get Stats
const getStats = async (token, params) => {
  try {
    setAuthToken(token);

    const queryParams = {
      from: params?.from
        ? new Date(params.from).toISOString()
        : new Date("2025-01-01").toISOString(),
      to: params?.to
        ? new Date(params.to).toISOString()
        : new Date().toISOString(),
    };


    const res = await api.get("/stats", { params: queryParams });
    return res.data;
  } catch (error) {
    return null;
  }
};

// get cashflows 
const getFlows = async (token) => {
  try {
    setAuthToken(token);
    const res = await api.get("/cashflows");
    return res.data;
  } catch (error) {
    return null;
  }
};

// get cats 
const getCategories = async (token) => {
  try {
    setAuthToken(token);
    const res = await api.get("/categories");
    return res.data;
  } catch (error) {
    return null;
  }
};

// get pay meth.
const getPayWays = async (token) => {
  try {
    setAuthToken(token);
    const res = await api.get("/payways");
    return res.data;
  } catch (error) {
    return null;
  }
};

const registerUser = async (u, p) => {
  try {
    const res = await axios.post(`${BASE}/users/register`, {
      username: u,
      password: p,
    });
    return res.data;
  } catch (error) {
    throw error;
  }
};

const loginUser = async (u, p) => {
  try {
    const res = await axios.post(`${BASE}/users/login`, {
      username: u,
      password: p,
    });

    sessionStorage.clear();
    sessionStorage.setItem("token", res.data.token);
    setAuthToken(res.data.token);

    return res.data;
  } catch (error) {
    throw error;
  }
};

// add and remove cashflows
const addFlow = async (token, flow) => {
  try {
    const res = await axios.post(`${BASE}/cashflows`, flow, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

const delFlow = async (token, id) => {
  try {
    const res = await axios.delete(`${BASE}/cashflows/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

// add and remove categories
const addCategory = async (token, cat) => {
  try {
    const res = await axios.post(`${BASE}/categories`, cat, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

const delCategory = async (token, id) => {
  try {
    const res = await axios.delete(`${BASE}/categories/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

// add and remove pay methods
const addPayWay = async (token, data) => {
  try {
    const res = await axios.post(`${BASE}/payways`, data, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

const delPayWay = async (token, id) => {
  try {
    const res = await axios.delete(`${BASE}/payways/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

// get budgets 
const getBudgets = async (token, params) => {
  try {
    const res = await axios.get(`${BASE}/budgets`, {
      headers: { Authorization: `Bearer ${token}` },
      params,
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

// add and remove budgets
const addBudget = async (token, data) => {
  try {
    const res = await axios.post(`${BASE}/budgets`, data, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

const delBudget = async (token, id) => {
  try {
    const res = await axios.delete(`${BASE}/budgets/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return res.data;
  } catch (error) {
    return null;
  }
};

export default {
  registerUser,
  loginUser,
  getFlows,
  addFlow,
  delFlow,
  getCategories,
  addCategory,
  delCategory,
  getPayWays,
  addPayWay,
  delPayWay,
  getBudgets,
  addBudget,
  delBudget,
  getStats,
};