import { Outlet } from 'react-router-dom'
import HomeHeader from '../Home/HomeHeader.tsx'

const PublicLayout = () => {
    return (
      <>
        <HomeHeader />
        <Outlet />
      </>
    )
  }

export default PublicLayout