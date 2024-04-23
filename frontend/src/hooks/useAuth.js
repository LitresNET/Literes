import axios from "axios";
import { useState } from "react";
import configData from "./../../config.json";

const useAuth = () => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const register = async (userData) => {
      setLoading(true);
      setError(null);

      try {
        const response = await axios.post(`${configData.BASE_URL}/user/signup`, userData);
        setLoading(false);
        if (response.status === 200) {
            return response.data
        } else {
            setError(response.data)
        }
      } catch (e) {
        setError(`Connection error: ${e}`);
        setLoading(false);
      }
    };

    const login = async (userData) => {
      setLoading(true);
      setError(null);

      try {
        const response = await axios.post(`${configData.BASE_URL}/user/signin`, userData);
        setLoading(false);
        if (response.status === 200) {
            return response.data
        } else {
            setError(response.data)
        }
      } catch (e) {
        setError(`Connection error: ${e}`);
        setLoading(false);
      }
    };

    return { loading, error, register, login };
  };

  export default useAuth;