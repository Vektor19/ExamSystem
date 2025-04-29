import React, { useState } from "react";
import { TextField, Box, Typography, Paper, Stack, Alert } from "@mui/material";
import { useAuth } from "../../Providers/AuthProvider";
import { RegisterUserRequest } from "../../Models/RegisterUserRequest";
import loginStyles from "../../Styles/LoginPage.module.css";
import PrimaryButton from "../Buttons/PrimaryButton";
import SecondaryButton from "../Buttons/SecondaryButton";
import { useNavigate } from "react-router-dom";

const RegisterForm: React.FC = () => {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [formData, setFormData] = useState<RegisterUserRequest>({
    firstname: "",
    lastname: "",
    email: "",
    password: "",
  });

  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage("");
    setSuccessMessage("");
    setIsSubmitting(true);

    const result = await register(formData);

    if (!result.success) {
      setErrorMessage(result.message);
      setIsSubmitting(false);
    } else {
      setSuccessMessage(result.message);
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
      <Typography variant="h5" textAlign="center" mb={2}>
        Create an Account
      </Typography>
      <Box component="form" onSubmit={handleSubmit} noValidate>
        <Stack spacing={2}>
          {errorMessage && <Alert severity="error">{errorMessage}</Alert>}
          {successMessage && <Alert severity="success">{successMessage}</Alert>}
          <TextField
            label="First Name"
            name="firstname"
            value={formData.firstname}
            onChange={handleChange}
            fullWidth
            required
          />
          <TextField
            label="Last Name"
            name="lastname"
            value={formData.lastname}
            onChange={handleChange}
            fullWidth
            required
          />
          <TextField
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            fullWidth
            required
          />
          <TextField
            label="Password"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
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
            Register
          </PrimaryButton>
        </Stack>
        <Stack direction="column" justifyContent="center">
          <Typography textAlign="center" variant="body1" mt={1} mb={1}>
            Already have an account?
          </Typography>
          <SecondaryButton
            size="small"
            variant="contained"
            className={loginStyles["small-login-form-button"]}
            onClick={() => {
              window.location.href = "/login";
            }}
          >
            Login
          </SecondaryButton>
        </Stack>
      </Box>
    </Paper>
  );
};

export default RegisterForm;
