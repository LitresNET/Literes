import { createBrowserRouter } from "react-router-dom";
import ComponentsLookup from "../components/componentsLookup";
import CheckoutPage from "../pages/CheckoutPages/CheckoutPage/CheckoutPage";
import SubscriptionPage from "../pages/SubscriptionPages/SubscriptionPage/SubscriptionPage";
import CustomSubscriptionPage from "../pages/SubscriptionPages/CustomSubscriptionPage/CustomSubscriptionPage";
import MainLayout from "../layouts/MainLayout/MainLayout";
import UserSettingsPage from "../pages/AccountPages/UserSettingsPage/UserSettingsPage";
import UserAccountPage from "../pages/AccountPages/UserAccountPage/UserAccountPage";
import PublisherPage from "../pages/AccountPages/PublisherPages/PublisherPage/PublisherPage";
import ModeratorPage from "../pages/AccountPages/ModeratorPage/ModeratorPage";
import BookSettingsPage from "../pages/MainPages/BookSettingsPage/BookSettingsPage";
import WelcomePage from "../pages/MainPages/WelcomePage/WelcomePage";
import SignInPage from "../pages/AuthPages/SignInPage/SignInPage.jsx";
import SignUpPage from "../pages/AuthPages/SignUpPage/SignUpPage.jsx";
import SearchPage from "../pages/MainPages/SearchPage/SearchPage";
import BookPage from "../pages/MainPages/BookPage/BookPage.jsx";
import NotFoundPage from "../pages/RedirectPages/NotFoundPage/NotFoundPage.jsx";
import OrderSuccessPage from "../pages/RedirectPages/OrderSuccessPage/OrderSuccessPage";
import UnauthorizedPage from "../pages/RedirectPages/UnauthorizedPage/UnauthorizedPage.jsx";

const routes = [
    {
      path: '/components',
      element: <ComponentsLookup />,
    },
    {
      path: '/',
      element: <MainLayout />,
      children: [
        {
          path:'',
          element: <WelcomePage />
        },
        {
          path:'home',
          element: <WelcomePage />
        },
        {
          path: '/success',
          element: <OrderSuccessPage />,
        },
        {
          path: '/unauthorized',
          element: <UnauthorizedPage />
        },
        {
          path: '/checkout',
          element: <CheckoutPage />,
        },
        {
          path: '/signin',
          element: <SignInPage />
        },
        {
          path: '/signup',
          element: <SignUpPage />
        },
        {
          path: '/search',
          element: <SearchPage />
        },
        {
          path: '/subscription',
          element: <SubscriptionPage />
        },
        {
          path: '/subscription/custom',
          element: <CustomSubscriptionPage />
        },
        {
          path: 'account',
          element: <UserAccountPage/>
        },
        {
          path: 'account/publisher',
          element: <PublisherPage/>
        },
        {
          path: 'account/moderator',
          element: <ModeratorPage/>
        },
        {
          path: 'account/settings',
          element: <UserSettingsPage/>
        },
        {
          path: 'account/publisher/book/:id/settings',
          element: <BookSettingsPage/>
        },
        {
          path: 'book/:id',
          element: <BookPage/>
        },
        {
          path: '*',
          element: <NotFoundPage />,
        }
      ]
    },
  ];

const router = createBrowserRouter(routes);

export default router;
