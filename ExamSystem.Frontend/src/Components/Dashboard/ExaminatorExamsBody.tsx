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
  Stack,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import PrimaryFab from "../Buttons/PrimaryFab";
import TimeUtils from "../../Utils/TimeUtils";
import DashboardPaper from "../Papers/DashboardPaper";

const ExaminatorExamsBody: React.FC = () => {
  const { examinatorExams } = useExams();
  const navigate = useNavigate();

  return (
    <>
      <DashboardPaper elevation={3} sx={{ p: 4, borderRadius: 3, overflowY: "auto" }}>
        <Stack spacing={2}>
          {examinatorExams?.length ? (
            <TableContainer
              sx={{
                width: "100%",
                overflowX: "auto",
              }}
            >
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell sx={{ fontWeight: "bold" }}>Name</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>Status</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>
                      Start Date
                    </TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>End Date</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>Questions</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>
                      Participants
                    </TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>Join Code</TableCell>
                    <TableCell align="right" sx={{ fontWeight: "bold" }}>
                      Actions
                    </TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {examinatorExams.map(
                    (exam) =>
                      exam && (
                        <TableRow key={exam.examId}>
                          <TableCell>{exam.name}</TableCell>
                          <TableCell>{exam.status}</TableCell>
                          <TableCell>
                            {TimeUtils.formatDate(exam.startDate)}
                          </TableCell>
                          <TableCell>
                            {TimeUtils.formatDate(exam.endDate)}
                          </TableCell>
                          <TableCell>{exam.questionCount}</TableCell>
                          <TableCell>{exam.participantCount}</TableCell>
                          <TableCell>{exam.joinCode}</TableCell>
                          <TableCell align="right">
                            <IconButton
                              onClick={() =>
                                navigate(`/dashboard/edit-exam/${exam.examId}`)
                              }
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
        </Stack>
      </DashboardPaper>

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
    </>
  );
};

export default ExaminatorExamsBody;
