import { Outlet } from 'react-router-dom'
import HomeNavigation from '../Home/HomeNavigation.tsx'

const PublicLayout = () => {
    return (
      <>
        <HomeNavigation />
        <Outlet />
      </>
    )
  }

export default PublicLayout