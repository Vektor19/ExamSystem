import React, { useState } from 'react';
import {
    TextField,
    Button,
    Box,
    Typography,
    Paper,
    Stack,
    Alert
} from '@mui/material';
import { useAuth } from '../../Providers/AuthProvider';
import { RegisterUserRequest } from '../../Models/RegisterUserRequest';

const RegisterForm: React.FC = () => {
    const { register } = useAuth();

    const [formData, setFormData] = useState<RegisterUserRequest>({
        firstname: '',
        lastname: '',
        email: '',
        password: '',
    });

    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrorMessage('');
        setSuccessMessage('');
        const result = await register(formData);

        if (!result.success) {
            setErrorMessage(result.message);
        } else {
            setSuccessMessage(result.message);
        }
    };

    return (
        <Paper
            elevation={3}
            sx={{
                maxWidth: 450,
                margin: 'auto',
                mt: 6,
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
                    <Button type="submit" variant="contained" fullWidth>
                        Register
                    </Button>
                </Stack>
            </Box>
        </Paper>
    );
};

export default RegisterForm;
