import Home from "./Home/Home.tsx";
import { Navigate, Route, Routes } from "react-router-dom";
import PublicLayout from "./Layouts/PublicLayout.tsx";

const App = () => {
  return (
    <>
      <Routes>
        <Route element={<PublicLayout />}>
          <Route path="/" element={<Home />} />
        </Route>
        {/* <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <DashboardLayout />
            </PrivateRoute>
          }
        >
          <Route index element={<Dashboard />} />
        </Route> */}
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </>
  );
};

export default App;
