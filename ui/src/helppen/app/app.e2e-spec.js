describe('App', function () {
    beforeEach(function () {
        browser.get('/');
    });

    it('should have a title', function () {
        expect(browser.getTitle()).toEqual("HELPPEN");
        browser.sleep(3000);
    });
});
