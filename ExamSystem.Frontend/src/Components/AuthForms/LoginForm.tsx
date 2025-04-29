import React, { useState } from "react";
import { TextField, Box, Typography, Paper, Stack, Alert } from "@mui/material";
import { useAuth } from "../../Providers/AuthProvider";
import PrimaryButton from "../Buttons/PrimaryButton";
import loginStyles from "../../Styles/LoginPage.module.css";
import SecondaryButton from "../Buttons/SecondaryButton";
import { useNavigate } from "react-router-dom";

const LoginForm: React.FC = () => {
  const { login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage("");
    setSuccessMessage("");
    setIsSubmitting(true);

    const result = await login(email, password);
    if (!result.success) {
      setErrorMessage(result.message || "Login Failed");
      setIsSubmitting(false);
    } else {
      setSuccessMessage(result.message || "Login successful");
      navigate("/dashboard");
    }
  };

  return (
    <Paper
      elevation={3}
      className={loginStyles["login-form"]}
      sx={{
        p: 4,
        borderRadius: 3,
      }}
    >
      <Typography variant="h5" textAlign="center" mb={4}>
        Login
      </Typography>
      <Box component="form" onSubmit={handleSubmit} noValidate>
        <Stack spacing={4}>
          {errorMessage && <Alert severity="error">{errorMessage}</Alert>}
          {successMessage && <Alert severity="success">{successMessage}</Alert>}
          <TextField
            label="Email"
            name="email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            fullWidth
            required
          />
          <TextField
            label="Password"
            name="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            fullWidth
            required
          />
          <PrimaryButton
            size="medium"
            type="submit"
            variant="contained"
            className={loginStyles["login-button"]}
            disabled={isSubmitting}
          >
            Login
          </PrimaryButton>
        </Stack>
        <Stack direction="column" justifyContent="center">
          <Typography textAlign="center" variant="body1" mt={1} mb={1}>
            Do not have an account?
          </Typography>
          <SecondaryButton
            size="small"
            variant="contained"
            className={loginStyles["small-login-form-button"]}
            onClick={() => {
              window.location.href = "/register";
            }}
          >
            Register
          </SecondaryButton>
        </Stack>
      </Box>
    </Paper>
  );
};

export default LoginForm;
