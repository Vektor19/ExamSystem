import { Exam } from "../../Models/Exam";
import { useExams } from "../../Providers/ExamsProvider";
import { useNavigate } from "react-router-dom";
import {
  Paper,
  Typography,
  Box,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import PrimaryFab from "../Buttons/PrimaryFab";
import styles from "../../Styles/ExaminatorExamsBody.module.css";

const ExaminatorExamsBody: React.FC = () => {
  const { examinatorExams } = useExams();
  const navigate = useNavigate();

  return (
    <div className={styles["exams-page-container"]}>
      <Box minHeight="80vh">
        <Paper elevation={3} sx={{ p: 4, borderRadius: 3 }}>
          {examinatorExams?.length ? (
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Start Date</TableCell>
                    <TableCell>End Date</TableCell>
                    <TableCell>Questions</TableCell>
                    <TableCell>Participants</TableCell>
                    <TableCell>Join Code</TableCell>
                    <TableCell align="right">Actions</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {examinatorExams.map(
                    (exam) =>
                      exam && (
                        <TableRow key={exam.examId}>
                          <TableCell>{exam.name}</TableCell>
                          <TableCell>{exam.status}</TableCell>
                          <TableCell>{exam.startDate}</TableCell>
                          <TableCell>{exam.endDate}</TableCell>
                          <TableCell>{exam.questionCount}</TableCell>
                          <TableCell>{exam.participantCount}</TableCell>
                          <TableCell>{exam.joinCode}</TableCell>
                          <TableCell align="right">
                            <IconButton
                              onClick={() => navigate(`/dashboard/edit-exam/${exam.examId}`)}
                              color="primary"
                            >
                              <EditIcon />
                            </IconButton>
                          </TableCell>
                        </TableRow>
                      )
                  )}
                </TableBody>
              </Table>
            </TableContainer>
          ) : (
            <Typography variant="body1" color="text.secondary">
              No exams found.
            </Typography>
          )}
        </Paper>

        <PrimaryFab
          color="primary"
          aria-label="add"
          size="large"
          sx={{
            position: "fixed",
            bottom: 24,
            right: 24,
            zIndex: 1000,
          }}
          onClick={() => navigate("/dashboard/create-exam")}
        >
          <AddIcon />
        </PrimaryFab>
      </Box>
    </div>
  );
};

export default ExaminatorExamsBody;
