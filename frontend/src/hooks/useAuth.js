import { useState } from "react";
import { axiosToLitres } from "./useAxios.js";

const useAuth = () => {
  const [loading, setLoading] = useState(false);

  async function register(userData) {
    setLoading(true);

    try {
      const response = await axiosToLitres.post(`/signup/user`, userData);
      setLoading(false);
      if (response.status === 200) {
        return { result: response.data, error: null };
      } else {
        return { result: null, error: response.data };
      }
    } catch (e) {
      setLoading(false);
      return { result: null, error: e.response.data.errors[0].description };
    }
  }

  async function login(userData) {
    setLoading(true);

    try {
      const response = await axiosToLitres.post(`/signin`, userData);
      setLoading(false);
      if (response.status === 200) {
        return { result: response.data, error: null };
      } else {
        return { result: null, error: response.data };
      }
    } catch (e) {
      setLoading(false);
      return { result: null, error: e.response.data.Message };
    }
  }

  return { loading, register, login };
};

  export default useAuth;