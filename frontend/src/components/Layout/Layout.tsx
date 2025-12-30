import React from 'react';
import Header from './Header';
import Navigation from './Navigation';
import Footer from './Footer';
import menuData from '../../Menu.json';
import { MenuItem } from '../../types/menu';
import styles from './Layout.module.css';

interface LayoutProps {
  children: React.ReactNode;
  userName?: string;
}

const Layout: React.FC<LayoutProps> = ({ children, userName }) => {
  const menuItems = menuData as MenuItem[];

  return (
    <div className={styles.layout} data-testid="layout">
      <Header userName={userName} />
      <Navigation menuItems={menuItems} />
      <main className={styles['main-content']} data-testid="main-content">
        {children}
      </main>
      <Footer />
    </div>
  );
};

export default Layout;
