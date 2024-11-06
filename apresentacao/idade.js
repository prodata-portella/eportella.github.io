(function age() {
    const calculate = (birthDate) => {
        const today = new Date();
        const birth = new Date(birthDate);
        let age = today.getFullYear() - birth.getFullYear();
        if (today.getMonth() < birth.getMonth() || (today.getMonth() === birth.getMonth() && today.getDate() < birth.getDate())) age--;
        return age;
    };
    const birthDate = '1985-06-28';
    document.getElementById('age').textContent = calculate(birthDate);
})()