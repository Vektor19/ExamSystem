import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Stack,
  Typography,
  Box,
} from "@mui/material";
import { useState } from "react";
import PrimaryButton from "../Buttons/PrimaryButton";
import SecondaryButton from "../Buttons/SecondaryButton";

interface Props {
  open: boolean;
  onClose: () => void;
  onSave: (joinCode: string) => void;
}

const JoinExamModal: React.FC<Props> = ({ open, onClose, onSave }) => {
  const [joinCode, setJoinCode] = useState<string>("");

  const handleJoinCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setJoinCode(e.target.value);
  };

  const joinExam = () => {
    if (joinCode.trim()) {
      onSave(joinCode);
      setJoinCode("");
      onClose();
    } else {
      alert("Please enter a valid code.");
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm">
      <Box p={2}>
        <DialogTitle>Join Exam</DialogTitle>
        <DialogContent>
          <Box display="flex" gap={3}>
            {/* --- Question Info --- */}
            <Stack spacing={2}>
              <Typography variant="subtitle1" fontWeight="bold">
                Enter Join Code
              </Typography>
              <TextField
                label="Code"
                name="joinCode"
                fullWidth
                value={joinCode}
                onChange={handleJoinCodeChange}
              />
            </Stack>
          </Box>
        </DialogContent>

        <DialogActions>
          <SecondaryButton onClick={onClose}>Cancel</SecondaryButton>
          <PrimaryButton variant="contained" onClick={joinExam}>
            Join
          </PrimaryButton>
        </DialogActions>
      </Box>
    </Dialog>
  );
};

export default JoinExamModal;
