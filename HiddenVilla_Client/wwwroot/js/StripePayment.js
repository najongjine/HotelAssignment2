redirectToCheckout = function (sessionId) {
  var stripe = Stripe('pk_test_51ItjxzF7koUBOIeZiNnfE3T3c0GSBn3xL5WBtpqVX65N6QK3Kf1U2Adn2rl8L4WkeC7xUy5Hpe71M9FnMexQ52ox00gy4hL3Uc');
  stripe.redirectToCheckout({
    sessionId: sessionId
  });
};