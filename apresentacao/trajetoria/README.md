# Nossa trajetória

Nasci em 24/06/2016 e fui constituida por **Ewerton da Silva Portella**, antes de falar sobre mim vou elencar a trajetória dele.

## Trajetória do Ewerton da Silva Portella

- Nascido em 28/06/1985 teve o seu primeiro contato com informática aproximandamente aos 10 anos com um computador pessoal da familia;
- Com 14 anos fez o seu primeiro curso de informática na [Data Byte](https://databyte.com.br/) de Windows Ms Office[^1];
- Com 16 anos na [Microcamp Tecnologia](https://microcamp.com.br/) concluiu o curso para montagem, manutenção e configuração de hardware.
- Também com 16 anos ouviu pela primeira primeira vez a profissão programador de computadores[^2] por um colega de infância que havia iniciado sua trajetória profissional como programador na [*Microsoft*](https://www.microsoft.com/);
- Entre os 17 e 26 anos construiu carreira no ramo de culinária oriental passando pelas empresas [*Flying Sushi*](https://flyingsushi.com.br/) e posteriormente [*Gendai*](https://flyingsushi.com.br/)[^3];
- Voltando aos 25 anos completou um módulo na [Impacta Certificações e Treinamentos](https://www.impacta.com.br/)[^4]
- Agora novamente aos 26 anos, Se matriculou na graduação Ciências da computação na [Uninove](https://www.uninove.br/) onde se formou; 
- Dos 27 até 30 anos atuou como Analista e desenvolvedor de *softwares*[^5];
- Por fim a partir dos 30 anos passou a complementar sua atuação coesistindo *softwares* com empreendedorismo;
- Hoje Ewerton da Silva Portella tem <span id="age">`idade`</span> anos.

## Minha trajetória
>[!NOTE]
>
>- Minha trajetória não é vazia. Estou preparando e em breve estará disponível.


[^1]: Primeiro contato com "programação" e não sabia muito o que fazer com aquilo.\
[^2]: Segundo contato com programação, mas estava com outros focos na vida.\
[^3]: Nessa fase passou por diversos cursos e treinamentos voltados a empreendedorismo e gestão de pessoas. Também foi meados a essa fase que o interesse por programação começou a surgir quando sentiu necessidade de eliminar procedimentos repetitivos como gerente e posteriomente gerente sócio em duas franquias na rede *Gendai*.
[^4]: O conteúdo do módulo foi, Introdução a Lógica de Programação. Introdução a Programação Orientada a Objetos. *MsSql Server 2008* Módulo I, *Java Programmer* Módulo I e II.
[^5]: Nesse periodo passou pela [IS2B](https://is2b.com.br), [Polícia Científica](https://www.policiacientifica.sp.gov.br/) pela **TODO!**, [ILATI](http://www.ilati.org.br/) e [FMU](https://fmu.br).


<script>

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

    (function blockquote(){
        const format = () => {
            document.querySelectorAll('blockquote p').forEach(p =>{
                if(p.textContent === '[!NOTE]')
                {
                    p.textContent = ''
                    p.style.display = 'flex'
                    p.style.alignItems = 'center'
                    p.style.columnGap = '0.4em'
                    const ns = "http://www.w3.org/2000/svg"

                    const note = document.createElementNS(ns, "svg")
                    note.setAttribute("viewBox", "0 0 16 16")
                    note.setAttribute("version", "1.1")
                    note.setAttribute("width", "16")
                    note.setAttribute("height", "16")
                    note.setAttribute("aria-hidden", "true")

                    const path = document.createElementNS(ns, "path")
                    path.setAttribute("d", "M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8Zm8-6.5a6.5 6.5 0 1 0 0 13 6.5 6.5 0 0 0 0-13ZM6.5 7.75A.75.75 0 0 1 7.25 7h1a.75.75 0 0 1 .75.75v2.75h.25a.75.75 0 0 1 0 1.5h-2a.75.75 0 0 1 0-1.5h.25v-2h-.25a.75.75 0 0 1-.75-.75ZM8 6a1 1 0 1 1 0-2 1 1 0 0 1 0 2Z")

                    note.appendChild(path)
                    p.appendChild(note)
                    p.append('Note')
                }
            })
        };
        format();
    })()
</script>