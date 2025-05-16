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
  onSave: (participantEmail: string) => void;
}

const AddParticipantModal: React.FC<Props> = ({ open, onClose, onSave }) => {
  const [participantEmail, setParticipantEmail] = useState<string>("");

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setParticipantEmail(e.target.value);
  };

  const addParticipant = () => {
    if (participantEmail.trim()) {
      onSave(participantEmail);
      setParticipantEmail("");
      onClose();
    } else {
      alert("Please enter a valid email address.");
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm">
      <Box p={2}>
        <DialogTitle>Add Participant</DialogTitle>
        <DialogContent>
          <Box display="flex" gap={3}>
            {/* --- Question Info --- */}
            <Stack spacing={2}>
              <Typography variant="subtitle1" fontWeight="bold">
                Participant email
              </Typography>
              <TextField
                label="Participant Email"
                name="participantEmail"
                fullWidth
                value={participantEmail}
                onChange={handleEmailChange}
              />
            </Stack>
          </Box>
        </DialogContent>

        <DialogActions>
          <SecondaryButton onClick={onClose}>Cancel</SecondaryButton>
          <PrimaryButton variant="contained" onClick={addParticipant}>
            Add Participant
          </PrimaryButton>
        </DialogActions>
      </Box>
    </Dialog>
  );
};

export default AddParticipantModal;
