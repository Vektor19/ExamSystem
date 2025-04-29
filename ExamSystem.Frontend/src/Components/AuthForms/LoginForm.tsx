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

const LoginForm: React.FC = () => {
    const { login } = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrorMessage('');
        const result = await login(email, password);
        if (!result.success) {
            setErrorMessage(result.message);
        } else {
            alert('Login successful!');
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
                Login
            </Typography>
            <Box component="form" onSubmit={handleSubmit} noValidate>
                <Stack spacing={2}>
                    {errorMessage && <Alert severity="error">{errorMessage}</Alert>}
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
                    <Button type="submit" variant="contained" fullWidth>
                        Login
                    </Button>
                </Stack>
            </Box>
        </Paper>
    );
};

export default LoginForm;
