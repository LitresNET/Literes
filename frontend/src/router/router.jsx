import { createBrowserRouter } from "react-router-dom";
import ComponentsLookup from "../components/componentsLookup";
import CheckoutPage from "../pages/CheckoutPages/CheckoutPage/CheckoutPage";
import DefaultSubscriptionPage from "../pages/SubscriptionPages/DefaultSubscriptionsPage/DefaultSubscriptionsPage";
import CustomSubscriptionPage from "../pages/SubscriptionPages/CustomSubscriptionPage/CustomSubscriptionPage";
import MainLayout from "../layouts/MainLayout/MainLayout";

const routes = [
    {
      path: '/components',
      element: <ComponentsLookup />,
    },
    {
      path: '/',
      element: <MainLayout/>,
      children: [
        {
          path: 'checkout',
          element: <CheckoutPage />,
        },
        {
          path: 'subscription',
          element: <DefaultSubscriptionPage />,
        },
        {
          path: 'subscription/custom',
          element: <CustomSubscriptionPage/>
        },
      ]
    },
  ];

  const router = createBrowserRouter(routes);
  export default router;