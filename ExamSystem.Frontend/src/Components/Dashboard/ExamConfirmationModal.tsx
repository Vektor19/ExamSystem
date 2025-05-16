import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControlLabel,
  Checkbox,
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
  onConfirm: () => void;
}

const ExamConfirmationModal: React.FC<Props> = ({ open, onClose, onConfirm }) => {
  const [checkedRules, setCheckedRules] = useState(false);

  const isValid = checkedRules;

  const handleClose = () => {
    setCheckedRules(false);
    onClose();
  };

  const handleStart = () => {
    if (isValid) {
      setCheckedRules(false);
      onConfirm();
    }
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm">
      <Box p={2}>
        <DialogTitle>Before You Start</DialogTitle>
        <DialogContent>
          <Stack spacing={2}>
            <Typography variant="body1">
              Please confirm the following before starting the exam:
            </Typography>
            <FormControlLabel
              control={
                <Checkbox
                  checked={checkedRules}
                  onChange={(e) => setCheckedRules(e.target.checked)}
                />
              }
              label="I have read and agree to the exam rules."
            />
          </Stack>
        </DialogContent>
        <DialogActions>
          <SecondaryButton onClick={handleClose}>Cancel</SecondaryButton>
          <PrimaryButton
            onClick={handleStart}
            disabled={!isValid}
          >
            Start
          </PrimaryButton>
        </DialogActions>
      </Box>
    </Dialog>
  );
};

export default ExamConfirmationModal;
