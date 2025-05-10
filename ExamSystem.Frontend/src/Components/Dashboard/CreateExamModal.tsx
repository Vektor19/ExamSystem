import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Stack,
} from "@mui/material";
import { useEffect, useState } from "react";
import { ExamCreate } from "../../Models/ExamCreate";
import { useUser } from "../../Providers/UserProvider";

interface Props {
  open: boolean;
  onClose: () => void;
  onCreate: (exam: ExamCreate) => void;
}

const CreateExamModal: React.FC<Props> = ({ open, onClose, onCreate }) => {
  const { user } = useUser();
  const [form, setForm] = useState<ExamCreate>({
    createdByUserId: "",
    name: "",
    startDate: "",
    endDate: "",
  });

  useEffect(() => {
    if (user) {
      setForm((prev) => ({ ...prev, createdByUserId: user.userId }));
    }
  }, [user]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };
  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: new Date(e.target.value) });
  };

  const handleSubmit = () => {
    onCreate(form);
    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Create Exam</DialogTitle>
      <DialogContent>
        <Stack spacing={2} mt={1}>
          <TextField
            name="name"
            label="Exam Name"
            fullWidth
            onChange={handleChange}
          />
          <TextField
            name="startDate"
            type="datetime-local"
            label="Start Date"
            InputLabelProps={{ shrink: true }}
            onChange={handleDateChange}
          />
          <TextField
            name="endDate"
            type="datetime-local"
            label="End Date"
            InputLabelProps={{ shrink: true }}
            onChange={handleDateChange}
          />
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmit} variant="contained">
          Create
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default CreateExamModal;
