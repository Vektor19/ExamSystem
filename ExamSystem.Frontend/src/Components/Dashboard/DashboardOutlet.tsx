import outletStyles from "../../Styles/DashboardOutlet.module.css";
import dashboardStyles from "../../Styles/Dashboard.module.css";
const DashboardOutlet = ({ children }: { children: React.ReactNode }) => {
  return (
    <>
      <section className={`${outletStyles["dashboard-outlet"]}`}>
        <div className={`${dashboardStyles["dashboard-page-container"]} `}>
          {children}
        </div>
      </section>
    </>
  );
};

export default DashboardOutlet;
