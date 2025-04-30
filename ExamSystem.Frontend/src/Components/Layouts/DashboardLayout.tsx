import { Outlet } from "react-router-dom";
import DashboardNavigation from "../Dashboard/DashboardNavigation.tsx";
import { DashboardProvider } from "../../Providers/DashboardProvider.tsx";

const DashboardLayout = () => {
  return (
    <>
      <DashboardProvider>
        <DashboardNavigation />
        <Outlet />
      </DashboardProvider>
    </>
  );
};

export default DashboardLayout;
