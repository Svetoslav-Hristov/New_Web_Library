using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using New_Web_Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Data.Configuration
{
    public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
    {

        private readonly Author[] initialAuthors =
            {
            new Author
            {
                Id= Guid.Parse("a1be31d1-8d08-4837-bf49-cf303d8083f0"),
                Name = "J.R.R. Tolkien",
                Biography="J.R.R. Tolkien (1892–1973) was an English writer, philologist, " +
                "and university professor best known as the author of The Hobbit and The Lord of the Rings," +
                " two of the most influential works of fantasy literature ever written. His imagination," +
                " world-building, and creation of original languages helped establish modern fantasy as a major" +
                " literary genre.\r\n\r\nHe was born John Ronald Reuel Tolkien in Bloemfontein, South Africa," +
                " and moved to England as a child after the death of his father. He grew up with a deep love" +
                " for language, mythology, and literature. Tolkien later studied at Exeter College," +
                " Oxford, where he specialized in classics and philology, the study of languages and their history." +
                "\r\n\r\nDuring World War I, Tolkien served in the British Army and took part in the Battle of the Somme." +
                " The hardships and losses of war deeply affected him and influenced themes of courage, friendship," +
                " sacrifice, and sorrow found in his later writing.\r\n\r\nAfter the war, " +
                "Tolkien became a respected academic. He taught at the University of Leeds and later at Oxford University," +
                " where he was a professor of Anglo-Saxon and then English Language and Literature. " +
                "He was an expert on medieval texts and translated important works such as Beowulf." +
                " His scholarly knowledge strongly shaped the myths, cultures, and languages of his fictional world." +
                "\r\n\r\nTolkien first gained fame with The Hobbit (1937), a children’s adventure novel following Bilbo Baggins" +
                " and his journey to recover treasure guarded by the dragon Smaug. Its success led to his masterpiece," +
                " The Lord of the Rings (1954–1955), an epic story set in Middle-earth involving the struggle against" +
                " the Dark Lord Sauron and the quest to destroy the One Ring.\r\n\r\nHis stories are admired for their depth," +
                " moral themes, memorable characters, and richly developed setting. Tolkien created entire histories, maps," +
                " races, and languages such as Elvish, making Middle-earth one of the most detailed fictional worlds ever imagined." +
                "\r\n\r\nHe was also a close friend of C.S. Lewis, and both were members of the literary discussion group known as the Inklings." +
                "\r\n\r\nJ.R.R. Tolkien died in 1973 in England. Today he is remembered as the father of modern fantasy" +
                " literature, whose works continue to inspire books, films, games, and generations of readers worldwide."
            },
            new Author
            {
                Id=Guid.Parse("a79aff9a-5b7d-4c86-8fdc-73950656cfd2"),
                Name="George Orwell",
                ImageUrl="/AuthorImages/Orwell.jpg",
                Biography="Orwell was a British journalist and author, who wrote two of the most famous novels of " +
                "the 20th century 'Animal Farm' and 'Nineteen Eighty-Four'.\r\n\r\n" +
                "Orwell was born Eric Arthur Blair on 25 June 1903 in eastern India, the son of a British colonial civil servant." +
                " He was educated in England and, after he left Eton, joined the Indian Imperial Police in Burma," +
                " then a British colony. He resigned in 1927 and decided to become a writer. In 1928," +
                " he moved to Paris where lack of success as a writer forced him into a series of menial jobs. " +
                "He described his experiences in his first book, 'Down and Out in Paris and London'," +
                " published in 1933. He took the name George Orwell, shortly before its publication. " +
                "This was followed by his first novel, 'Burmese Days', in 1934.\r\n\r\nAn anarchist in the late 1920s," +
                " by the 1930s he had begun to consider himself a socialist. In 1936, he was commissioned to" +
                " write an account of poverty among unemployed miners in northern England, which resulted in " +
                "'The Road to Wigan Pier' (1937). Late in 1936, Orwell travelled to Spain to fight for the Republicans against Franco's " +
                "Nationalists. He was forced to flee in fear of his life from Soviet-backed communists who were" +
                " suppressing revolutionary socialist dissenters. The experience turned him into a lifelong " +
                "anti-Stalinist.\r\n\r\nBetween 1941 and 1943, Orwell worked on propaganda for the BBC. " +
                "In 1943, he became literary editor of the Tribune, a weekly left-wing magazine. " +
                "By now he was a prolific journalist, writing articles, reviews and books.\r\n\r\nIn 1945, Orwell's " +
                "'Animal Farm' was published. A political fable set in a farmyard but based on Stalin's betrayal " +
                "of the Russian Revolution, it made Orwell's name and ensured he was financially " +
                "comfortable for the first time in his life. 'Nineteen Eighty-Four' was published four years later. " +
                "Set in an imaginary totalitarian future, the book made a deep impression, with its title and many phrases " +
                "- such as 'Big Brother is watching you', 'newspeak' and 'doublethink' - entering popular use." +
                " By now Orwell's health was deteriorating and he died of tuberculosis on 21 January 1950."


            },
            new Author
            {
                Id=Guid.Parse("1dc33818-603a-4c00-ba62-93c819bf3ba7"),
                Name="Aldous Huxley",
                ImageUrl="/AuthorImages/Aldous_Huxley.jpg",
                Biography="Aldous Huxley (1894–1963) was an English writer, essayist, and intellectual best known for his influential dystopian novel " +
                "Brave New World (1932). Born in Godalming, Surrey, England, he came from a distinguished family of scientists and " +
                "thinkers. His grandfather was the famous biologist Thomas Henry Huxley, known as “Darwin’s Bulldog,”" +
                " and his brother Julian Huxley later became a renowned biologist and the first Director-General " +
                "of UNESCO.\r\n\r\nHuxley studied at Eton College and later at Balliol College, Oxford, " +
                "where he read English literature. During his youth, he suffered from a severe eye illness that" +
                " temporarily left him nearly blind, an experience that deeply affected his life and limited some " +
                "career possibilities. Despite this challenge, he developed into a prolific author.\r\n\r\nIn the 1920s, " +
                "Huxley gained recognition for his sharp social satire in novels such as Crome Yellow and Point Counter Point." +
                " His most famous work, Brave New World, imagined a technologically advanced society in which people " +
                "are controlled through conditioning, consumerism, and pleasure rather than force. The novel became one " +
                "of the most important warnings about authoritarianism, mass culture, and the misuse of science.\r\n\r\nLater" +
                " in life, Huxley moved to the United States and lived in California. There he became increasingly interested " +
                "in philosophy, spirituality, mysticism, and human consciousness. These themes appeared in works such" +
                " as The Perennial Philosophy (1945) and The Doors of Perception (1954), which explored altered states" +
                " of mind and influenced later cultural movements.\r\n\r\nHuxley also worked as a screenwriter in Hollywood" +
                " for a time, contributing to several film projects. Throughout his life, he remained a thoughtful critic of modern " +
                "society, technology, and the dangers of losing individuality.\r\n\r\nHe died on November 22, 1963, in Los Angeles," +
                " California. Aldous Huxley is remembered as one of the twentieth century’s most important writers, whose works continue" +
                " to inspire debate about freedom, ethics, science, and the future of humanity."

            },
            new Author
            {
                Id=Guid.Parse("86fe4481-0fe8-44fc-9ca1-7f53edf0dab8"),
                Name="Robert C. Martin",
                ImageUrl="/AuthorImages/RobertMartin.jpg",
                Biography="Robert C. Martin, widely known as “Uncle Bob,” is an American software engineer, " +
                "author, and speaker best known for his major influence on modern software development practices." +
                " He is one of the most recognized advocates of clean code, agile principles, " +
                "and professional standards in programming.\r\n\r\nMartin began his career as a " +
                "programmer in the early years of the software industry and accumulated decades of " +
                "practical experience in designing and building software systems. Over time, he " +
                "became known not only for technical expertise but also for his strong views on craftsmanship, " +
                "discipline, and responsibility in software engineering.\r\n\r\nHe was one of the original " +
                "signatories of the Agile Manifesto in 2001, a landmark document that helped transform the way " +
                "software teams work. Agile development emphasized collaboration, adaptability," +
                " frequent delivery, and close communication with customers. Martin played an important role in spreading " +
                "these ideas through books, lectures, and consulting.\r\n\r\nHe is the author of several" +
                " highly influential books. His best-known work, Clean Code: A Handbook of Agile Software " +
                "Craftsmanship (2008), became a standard reference for developers around the world. In it," +
                " he promoted writing readable, maintainable, and well-structured code." +
                " Other notable books include The Clean Coder, Clean Architecture, and Agile Software Development: " +
                "Principles, Patterns, and Practices.\r\n\r\nMartin is also strongly associated" +
                " with the SOLID principles of object-oriented design, which help developers " +
                "create flexible and maintainable software systems. These principles are widely taught" +
                " and used in professional software development.\r\n\r\nThrough conferences, training sessions," +
                " articles, and online talks, he has mentored generations of programmers." +
                " His teaching style combines technical depth with practical advice about ethics, teamwork, " +
                "testing, and long-term code quality.\r\n\r\nRobert C. Martin remains one of the most " +
                "influential figures in software engineering. His ideas continue to shape how developers " +
                "think about code structure, professionalism, " +
                "and building software that can evolve successfully over time."

            },
            new Author
            {
                Id=Guid.Parse("47df0e8f-7a6b-4ed1-abfa-372f26dbcd49"),
                Name="Andrew Hunt",
                ImageUrl="/AuthorImages/Andy_Hunt.jpg",

            },
            new Author
            {
                Id=Guid.Parse("d7cf5390-d958-4fdd-8365-d019cac4e850"),
                Name="Dan Brown",
                ImageUrl="/AuthorImages/DanBrown.jpg",
                Biography="Dan Brown (born 1964) is an American author best known for his bestselling thriller novels" +
                " that combine mystery, history, symbols, art, and secret organizations. He became one of the most " +
                "commercially successful writers of the modern era through stories that blend fast-paced suspense " +
                "with intellectual puzzles.\r\n\r\nHe was born in Exeter, New Hampshire, United States. His father" +
                " was a mathematics teacher, and his mother was a musician and church organist. Growing up in an academic " +
                "environment influenced his later interest in codes, logic, religion, and hidden meanings—topics that became" +
                " central elements in his fiction.\r\n\r\nBrown studied at Amherst College and " +
                "later spent time in Europe, where he developed a strong interest in art history and culture. " +
                "Before becoming a full-time writer, he worked as a teacher and pursued music. His early novels gained moderate attention," +
                " but his breakthrough came in 2003 with The Da Vinci Code.\r\n\r\nThe Da Vinci Code became " +
                "an international phenomenon, selling millions of copies worldwide. The novel follows Harvard symbologist " +
                "Robert Langdon as he investigates a murder in the Louvre Museum and uncovers clues connected to religious history," +
                " secret societies, and famous works of art. Its success led to global debates, controversy, and major interest in historical " +
                "mysteries.\r\n\r\nBrown continued the Robert Langdon series with novels such as Angels & Demons, The Lost Symbol, Inferno," +
                " and Origin. These books often explore themes such as science versus religion, technology, cryptography, architecture, and hidden" +
                " knowledge.\r\n\r\nSeveral of his novels were adapted into successful films starring Tom Hanks as" +
                " Robert Langdon, helping expand Brown’s worldwide popularity.\r\n\r\nDan Brown is known for his short" +
                " chapters, cliffhangers, and puzzle-driven storytelling style that keeps readers engaged. Although some historians and " +
                "critics have challenged the accuracy of certain claims in his fiction, his books have inspired many readers to explore art," +
                " history, architecture, and symbolism.\r\n\r\nHe remains one of the most recognized contemporary thriller writers, whose works" +
                " continue to attract audiences around the world."

            },
            new Author
            {
                Id=Guid.Parse("4841f823-58e2-4aea-98b9-3cdbd9b740e4"),
                Name="Arthur Conan Doyle",
                ImageUrl="/AuthorImages/ArturConnonDoile.jpg",
                Biography="Arthur Conan Doyle (1859–1930) was a British writer and physician best known as the" +
                " creator of the legendary detective Sherlock Holmes, one of the most famous fictional characters" +
                " in literary history. His works helped shape modern detective fiction and " +
                "continue to influence literature, film, and television around the world.\r\n\r\nHe was born in Edinburgh," +
                " Scotland, into an Irish Catholic family. Doyle studied medicine at the University of Edinburgh," +
                " where one of his professors, Dr. Joseph Bell, impressed students with his remarkable powers of" +
                " observation and deduction. Bell later became an important inspiration for the character" +
                " of Sherlock Holmes.\r\n\r\nAfter qualifying as a doctor, Doyle worked as a physician while also writing " +
                "stories in his spare time. In 1887 he published A Study in Scarlet, the first novel featuring Sherlock Holmes and Dr." +
                " John Watson. Holmes’s brilliant reasoning, scientific methods, and sharp attention to detail quickly captured readers’" +
                " interest.\r\n\r\nDoyle later wrote many more Holmes adventures, including The Sign of Four, The Hound of the Baskervilles," +
                " and dozens of short stories published in magazines. Sherlock Holmes became so popular that when Doyle attempted to end the series," +
                " public demand eventually persuaded him to bring the detective back.\r\n\r\nAlthough Holmes " +
                "overshadowed much of his other work, Doyle also wrote historical novels, science fiction, adventure stories," +
                " and nonfiction. His Professor Challenger stories, including The Lost World (1912), " +
                "became classics of early science fiction and adventure literature.\r\n\r\nOutside writing, " +
                "Doyle was active in public life and supported causes he believed in, sometimes using his fame to " +
                "campaign against miscarriages of justice. In later life he developed a strong interest in spiritualism " +
                "and wrote extensively on the subject.\r\n\r\nArthur Conan Doyle was knighted in 1902 for his public service" +
                " and contributions during the Boer War period. He died on July 7, 1930, in England.\r\n\r\nToday he is remembered" +
                " as one of the most influential storytellers of the modern era. Through Sherlock Holmes, Doyle established many of" +
                " the detective story conventions still used today, including logical deduction, forensic clues," +
                " and the brilliant investigator solving seemingly impossible mysteries."

            },
            new Author
            {
                Id=Guid.Parse("732f4066-8eb3-4ede-b7c5-5edf339574ac"),
                Name="Stephen King",
                ImageUrl="/AuthorImages/StephenKing.jpg",
                Biography="Stephen King (born 1947) is an American author widely regarded as one of the most" +
                " successful and influential writers of modern popular fiction. He is best known for his horror novels," +
                " but his work also includes suspense, fantasy, science fiction, drama, and psychological thrillers." +
                " Over his long career, he has published dozens of bestselling books and sold hundreds of millions of copies worldwide." +
                "\r\n\r\nHe was born in Portland, Maine, United States," +
                " and was raised primarily by his mother after his father left the family when King was young." +
                " From an early age, he developed a love of storytelling, horror films, and imaginative fiction." +
                " He studied English at the University of Maine, " +
                "where he also began publishing short stories.\r\n\r\nBefore achieving literary success, " +
                "King worked various jobs, including teaching. " +
                "His breakthrough came in 1974 with the publication of Carrie, " +
                "a novel about a troubled teenage girl with supernatural powers. " +
                "The book became a major success and was later adapted into a popular film." +
                "\r\n\r\nKing went on to write many famous novels, including The Shining, " +
                "Salem’s Lot, It, Misery, Pet Sematary, The Stand, and Doctor Sleep. His stories often " +
                "combine ordinary settings with terrifying or supernatural events, creating fear through both" +
                " atmosphere and character psychology. He is especially admired for making readers care deeply" +
                " about his characters before placing them in extreme situations.\r\n\r\nIn addition to horror," +
                " King has written acclaimed works such as The Green Mile, 11/22/63, and novellas like Rita Hayworth" +
                " and Shawshank Redemption and The Body, which inspired successful films. Several of his books have" +
                " been adapted into movies, television series, and stage productions.\r\n\r\nKing survived a serious" +
                " accident in 1999 when he was struck by a vehicle while walking near his home, but he recovered and " +
                "continued writing. He has also written essays and nonfiction, including On Writing, a respected book about " +
                "the craft of writing.\r\n\r\nStephen King remains one of the defining voices of modern fiction. His storytelling ability," +
                " memorable characters, and enormous range have earned him generations of readers and a lasting place" +
                " in contemporary literature."
            },
            new Author
            {
                Id=Guid.Parse("86867d10-13a6-41cd-91c3-eac7c74d0267"),
                Name="Paulo Coelho",
                ImageUrl="/AuthorImages/Paulo_Coelho.jpg",
                Biography="Paulo Coelho (born 1947) is a Brazilian author best known for his internationally acclaimed novel" +
                " The Alchemist, one of the most widely read and translated books in modern literature." +
                " His works often explore themes such as destiny, spirituality, personal growth, " +
                "love, and the search for meaning in life.\r\n\r\nHe was born in Rio de Janeiro, Brazil." +
                " During his youth, Coelho had a difficult relationship with his family, who initially opposed " +
                "his artistic ambitions and preferred a more conventional career path for him. He showed an early " +
                "interest in literature, theatre, and creative expression. Before becoming a novelist, he worked in several fields," +
                " including journalism, songwriting, and theatre.\r\n\r\nIn the 1970s, Coelho became known in Brazil as a lyricist" +
                " and collaborated with famous musicians, writing songs that brought him public recognition. During this period," +
                " he also experienced political repression under Brazil’s military dictatorship, an experience that influenced his" +
                " later views on freedom and self-discovery.\r\n\r\nA turning point in his life came when he undertook a spiritual" +
                " pilgrimage across northern Spain on the Camino de Santiago. This journey inspired his autobiographical book" +
                " The Pilgrimage (1987), which explored faith, discipline, and inner transformation.\r\n\r\nSoon after," +
                " he published The Alchemist (1988), the story of a young shepherd named Santiago who travels in search" +
                " of treasure and learns to follow his dreams. Though modestly successful at first, the novel later became" +
                " a global phenomenon. It has been translated into many languages and has inspired millions of readers around the world." +
                "\r\n\r\nCoelho continued writing bestselling novels such as Brida, Veronika Decides to Die, Eleven Minutes, The Valkyries," +
                " and Adultery. His books often combine simple storytelling with philosophical and spiritual ideas, making" +
                " them accessible to a broad international audience.\r\n\r\nHe is also known for being active online and maintaining" +
                " close communication with readers through social media. Beyond literature, Coelho has supported charitable and cultural initiatives." +
                "\r\n\r\nPaulo Coelho remains one of the most influential contemporary authors. His messages about courage, purpose, and listening " +
                "to one’s heart have given his books lasting global popularity."

            },
            new Author
            {
                Id=Guid.Parse("98be7892-9c29-45b0-bc44-3a55f55438d8"),
                Name="Yuval Noah Harari",
                ImageUrl="/AuthorImages/Yuval_Noah_Harari.jpg"
            },
            new Author
            {
                Id=Guid.Parse("1e1e3c27-c460-4fc8-9e00-a799d28f3c71"),
                Name="Walter Isaacson",
                ImageUrl="/AuthorImages/Walter_Isaacson.jpg"
            },
            new Author
            {
                Id=Guid.Parse("3be2562a-695c-41c1-b3f7-713e4bda5da0"),
                Name="Andy Weir",
                ImageUrl="/AuthorImages/Andy_Weir.jpg"
            },
            new Author
            {
                Id=Guid.Parse("383e7342-18e8-4eb8-a74e-9b403011d328"),
                Name="Frank Herbert",
                ImageUrl="/AuthorImages/Frank_Herbert.jpg"
            },
            new Author
            {
                Id=Guid.Parse("c8f65ff4-c963-4397-be53-6cda4c7fb567"),
                Name="Patrick Rothfuss",
                ImageUrl="/AuthorImages/Patrick_Rothfuss.jpg"
            },
            new Author
            {
                Id=Guid.Parse("527255b7-2a1b-486a-b5be-6ebf376487f2"),
                Name="Harper Lee",
                ImageUrl="/AuthorImages/Harper-Lee.jpg"
            },
            new Author
            {
                Id=Guid.Parse("49e43a4b-106a-4ad0-9ecc-1234064f6cde"),
                Name="Jane Austen",
                ImageUrl="/AuthorImages/Jane_Austen.jpg",
                Biography="Jane Austen (1775–1817) was an English novelist celebrated for her sharp social observation," +
                " wit, and enduring stories about love, marriage, family, and class." +
                " She is considered one of the most important writers in English literature, and her novels" +
                " remain widely read and adapted around the world.\r\n\r\nShe was born in Steventon, Hampshire," +
                " England, into a close and educated family. Her father was a clergyman who encouraged reading and learning," +
                " giving Austen access to a broad range of books from an early age. She began writing as a teenager," +
                " producing stories, parodies, and early drafts that revealed her intelligence and humor.\r\n\r\nAusten" +
                " lived during a period when women had limited economic independence, and marriage often shaped a woman’s future." +
                " These realities strongly influenced her fiction. Rather than writing about wars or grand politics, she focused" +
                " on domestic life, relationships, manners, and the social pressures of her time.\r\n\r\nHer major novels include" +
                " Sense and Sensibility (1811), Pride and Prejudice (1813), Mansfield Park (1814), and Emma (1815). In these works," +
                " Austen created memorable heroines such as Elizabeth Bennet and Emma Woodhouse, characters admired for their intelligence," +
                " individuality, and emotional growth. Her stories often combine romance with subtle criticism of vanity," +
                " hypocrisy, and social ambition.\r\n\r\nAusten’s novels were originally published anonymously," +
                " often identified only as being written “By a Lady.” Although she achieved some success during her lifetime," +
                " her full fame grew significantly after her death.\r\n\r\nShe completed Persuasion and Northanger Abbey," +
                " both of which were published posthumously in 1818. These later works further demonstrated" +
                " her maturity as a writer and her skill in portraying human relationships.\r\n\r\nJane Austen died" +
                " in Winchester, England, at the age of 41. Though her life was relatively quiet, her literary legacy became immense." +
                " Her novels are praised for elegant prose, psychological insight, and timeless themes that continue " +
                "to resonate with readers across generations.\r\n\r\nToday Jane Austen is remembered as a master of the novel" +
                " whose works continue to inspire films, television adaptations, academic study, and devoted" +
                " readers worldwide."

            },
            new Author
            {
                Id=Guid.Parse("e93c976c-b126-4cf9-a3ee-b816c250d7aa"),
                Name="F. Scott Fitzgerald",
                ImageUrl="/AuthorImages/F._Scott_Fitzgerald.jpg",
                Biography="F. Scott Fitzgerald (1896–1940) was an American novelist and short story writer best" +
                " known for capturing the spirit, glamour, and disillusionment of the Jazz Age." +
                " He is considered one of the greatest writers of twentieth-century American" +
                " literature, with works that explore wealth, ambition, love, and the fragility of" +
                " the American Dream.\r\n\r\nHe was born Francis Scott Key Fitzgerald in Saint Paul, Minnesota," +
                " United States. From a young age, he showed literary talent and strong ambition. " +
                "He attended Princeton University, where he wrote for student publications and developed his passion for writing," +
                " though he left before graduating to join the U.S. Army during World War I." +
                "\r\n\r\nWhile stationed in the military, Fitzgerald met Zelda Sayre, who would later become " +
                "his wife and one of the most famous figures of the 1920s. Their marriage became both glamorous and" +
                " turbulent, marked by fame, financial pressure, and personal struggles.\r\n\r\nFitzgerald achieved " +
                "immediate success with his first novel, This Side of Paradise (1920)," +
                " which made him a literary celebrity. The book captured the attitudes and energy of" +
                " a new postwar generation. He followed it with novels such as The Beautiful and Damned and " +
                "Tender Is the Night, along with many short stories published in leading magazines.\r\n\r\nHis masterpiece," +
                " The Great Gatsby (1925), is now regarded as one of the finest American novels ever written. " +
                "Set during the roaring twenties, it tells the story of Jay Gatsby, a mysterious millionaire driven by" +
                " love and illusion. The novel examines class, desire, moral emptiness, and the pursuit of impossible dreams." +
                "\r\n\r\nDespite his talent, Fitzgerald struggled with financial difficulties, heavy drinking, and the mental illness of Zelda," +
                " who spent much of her later life in psychiatric care. In the 1930s he moved to Hollywood," +
                " where he worked as a screenwriter while continuing to write fiction.\r\n\r\nHe died in 1940 at the age of 44," +
                " believing much of his success had faded. However, after his death, his reputation grew enormously, especially " +
                "through the rediscovery of The Great Gatsby.\r\n\r\nToday F. Scott Fitzgerald is remembered as a brilliant" +
                " stylist and chronicler of ambition, beauty, and disappointment. His works continue to define an era and remain" +
                " central to American literary culture."
            },
            new Author
            {
                Id=Guid.Parse("d6f1d900-f138-4ba9-b956-da3738d7f988"),
                Name="Fyodor Dostoevsky",
                ImageUrl="/AuthorImages/Fiodor_Dostoievski.jpg",
                Biography="Fyodor Dostoevsky (1821–1881) was a Russian novelist, philosopher," +
                " and journalist widely regarded as one of the greatest writers in world literature. " +
                "His works explored psychology, morality, faith, freedom, suffering, and the inner conflicts of human nature." +
                " He is especially admired for the emotional depth and philosophical power of his novels.\r\n\r\nHe was born" +
                " in Moscow, Russia, into the family of a military doctor. Dostoevsky studied engineering at the" +
                " Nikolayev Military Engineering Institute in Saint Petersburg, but his true passion was literature." +
                " After completing his studies, he left engineering service and turned to writing.\r\n\r\nHis first novel," +
                " Poor Folk (1846), gained immediate attention and established him as a promising young writer. However," +
                " his life soon took a dramatic turn. In 1849 he was arrested for participating in a discussion group that criticized" +
                " the Russian government. He was sentenced to death, but at the last moment the execution was canceled and replaced with" +
                " years of hard labor in Siberia, followed by military service.\r\n\r\nThis traumatic experience deeply" +
                " shaped his worldview and later writing. Themes of punishment, redemption, guilt, and spiritual rebirth became central" +
                " in his novels.\r\n\r\nAfter returning from exile, Dostoevsky wrote many of his greatest works, including Notes from Underground" +
                ", Crime and Punishment, The Idiot, Demons (also known as The Possessed), and The Brothers Karamazov." +
                " These novels examine moral choices, crime, religious belief, social unrest, and the search for meaning in a" +
                " troubled world.\r\n\r\nCrime and Punishment tells the story of Rodion Raskolnikov, a poor student who commits " +
                "murder and struggles with guilt and conscience. The Brothers Karamazov, his final major novel, is considered one of" +
                " the greatest philosophical novels ever written.\r\n\r\nDostoevsky also struggled personally with debt, epilepsy, and" +
                " gambling addiction, yet continued to write with extraordinary intensity. His difficult life experiences " +
                "gave his fiction unusual realism and psychological insight.\r\n\r\nHe died in Saint Petersburg in 1881. " +
                "Today Fyodor Dostoevsky is remembered as a literary giant whose exploration of the human soul influenced writers," +
                " psychologists, philosophers, and readers throughout the world."
            },
            new Author
            {
                Id=Guid.Parse("9389518b-498c-4bbb-974a-079794394e2e"),
                Name="J.D. Salinger",
                ImageUrl="/AuthorImages/J_D_Salinger.jpg",
                Biography="J.D. Salinger (1919–2010) was an American writer best known for his influential novel" +
                " The Catcher in the Rye (1951), one of the most widely read and discussed books of the twentieth" +
                " century. He became famous for his distinctive voice, " +
                "sensitive portrayal of youth, and later for his extraordinary privacy " +
                "and withdrawal from public life.\r\n\r\nHe was born Jerome David Salinger in New York City, " +
                "United States. He grew up in Manhattan and later attended several schools before enrolling" +
                " at Valley Forge Military Academy. During his early adulthood, he developed a serious interest " +
                "in writing and studied creative writing while beginning to publish short stories." +
                "\r\n\r\nSalinger served in the United States Army during World War II and took part " +
                "in major military campaigns in Europe, including the Normandy invasion." +
                " His wartime experiences deeply affected him and influenced the emotional tone of some " +
                "of his later work.\r\n\r\nHis literary breakthrough came with The Catcher in the Rye, " +
                "a novel narrated by teenager Holden Caulfield. The story follows Holden’s wandering days" +
                " in New York after leaving school and explores alienation, identity, " +
                "innocence, and dissatisfaction with the adult world. The novel’s honest language " +
                "and rebellious perspective strongly connected with generations of young readers." +
                "\r\n\r\nAfter the success of the novel, Salinger published important collections such as Nine Stories" +
                " and books centered on the Glass family, including Franny and Zooey and Raise High the Roof Beam, " +
                "Carpenters and Seymour: An Introduction. These works showed his interest in spirituality, " +
                "family dynamics, and psychological complexity.\r\n\r\nDespite enormous fame, Salinger increasingly withdrew" +
                " from public attention. He moved to Cornish, New Hampshire, where he lived privately for decades, rarely giving" +
                " interviews or publishing new work. His reclusive lifestyle became almost as famous as his writing." +
                "\r\n\r\nHe died in 2010 at the age of 91. Although he published relatively little compared with" +
                " many major authors, his impact on literature was significant. His exploration of adolescence," +
                " authenticity, and emotional vulnerability continues to influence writers and readers around the world." +
                "\r\n\r\nToday J.D. Salinger is remembered as both a major literary voice and one of the most mysterious figures " +
                "in modern American literature."
            },
            new Author
            {
                Id=Guid.Parse("06c9d74c-a4c6-4218-8d41-e65cf98be9b9"),
                Name="Bram Stoker",
                ImageUrl="/AuthorImages/Bram_Stroker.jpg",
                Biography="Bram Stoker (1847–1912) was an Irish author and theatre manager best known" +
                " for his classic Gothic horror novel Dracula (1897), one of the most influential works in" +
                " horror literature. Through this novel, he created one of the most enduring figures in popular " +
                "culture and helped define the modern image of the vampire.\r\n\r\nHe was born Abraham Stoker in Clontarf," +
                " Dublin, Ireland. As a child, he suffered from illness and spent much of his early years bedridden. " +
                "During this time he developed a strong imagination and love of storytelling. Later," +
                " he recovered and went on to study mathematics at Trinity College Dublin, where he also became active in" +
                " athletics and student life.\r\n\r\nAfter university, Stoker worked as a civil servant while writing theatre criticism." +
                " His reviews brought him into contact with the famous actor Sir Henry Irving. Stoker later became Irving’s manager and moved" +
                " to London, where he spent many years managing the Lyceum Theatre. This career placed him in artistic and literary circles" +
                " and influenced his writing.\r\n\r\nAlthough he wrote several novels and short stories, his greatest" +
                " success came with Dracula. Told through letters, diary entries, and documents, the novel follows Count" +
                " Dracula’s attempt to spread his power from Transylvania to England and the efforts of a group led by Professor " +
                "Van Helsing to stop him. The story combined folklore, suspense, superstition, and modern fears of the Victorian age." +
                "\r\n\r\nDracula was not an immediate worldwide sensation on publication, but over time it became a literary " +
                "masterpiece and the foundation of countless films, plays, television series, and adaptations. Count Dracula became" +
                " one of the most recognizable fictional villains in history.\r\n\r\nStoker also wrote other works, including The Jewel" +
                " of Seven Stars, The Lair of the White Worm, and adventure stories, though none achieved the fame of Dracula." +
                "\r\n\r\nHe died in London in 1912. After his death, the popularity of Dracula continued to grow enormously.\r\n\r\nToday" +
                " Bram Stoker is remembered as a central figure in Gothic fiction and horror literature. His imagination shaped " +
                "modern vampire mythology and left a lasting mark on world culture."
            },
            new Author
            {
                Id=Guid.Parse("3aee53ce-3292-4895-8cef-5c41ab9684db"),
                Name="Ray Bradbury",
                ImageUrl="/AuthorImages/Ray_Bradbury.jpg",
                Biography="Ray Bradbury (1920–2012) was an American author celebrated for his imaginative works" +
                " of science fiction, fantasy, horror, and literary fiction. He is best known for" +
                " combining poetic language with powerful ideas about technology, censorship, human nature," +
                " and the importance of imagination. Bradbury became one of the most beloved and influential" +
                " writers of the twentieth century.\r\n\r\nHe was born in Waukegan, Illinois, United States." +
                " Many memories of his childhood in a small town later inspired the nostalgic settings and" +
                " emotional tone of some of his fiction. His family later moved to Los Angeles," +
                " California, where Bradbury developed a deep love for libraries, cinema, and storytelling." +
                " Because he could not afford college, he educated himself through extensive reading and lifelong " +
                "study in public libraries.\r\n\r\nBradbury began publishing stories" +
                " in magazines during the 1940s and quickly gained attention for his originality and vivid style." +
                " His breakthrough came with The Martian Chronicles (1950), a connected series of stories about human settlement on Mars." +
                " The book explored colonization, loneliness, cultural conflict, and the destruction of innocence.\r\n\r\nHis most" +
                " famous novel, Fahrenheit 451 (1953), imagined a future society where books are banned and “firemen”" +
                " burn them. The story follows Guy Montag, a man who begins to question censorship and conformity." +
                " The novel became a classic warning about the dangers of suppressing ideas and losing independent thought." +
                "\r\n\r\nBradbury also wrote Something Wicked This Way Comes, Dandelion Wine, The Illustrated Man, " +
                "and hundreds of short stories. His work often blended wonder, fear, nostalgia, and moral reflection." +
                " He was especially skilled at portraying both the beauty and danger of technological progress.\r\n\r\nBeyond books," +
                " Bradbury wrote screenplays, essays, plays, and television scripts. He contributed to film and popular culture," +
                " including work connected with adaptations of his stories. He remained active for decades as a " +
                "public speaker and defender of reading, libraries, and creativity.\r\n\r\nRay Bradbury died in 2012 in Los Angeles. " +
                "Today he is remembered as one of America’s great storytellers, whose works continue to " +
                "inspire readers to value imagination, freedom, and the human spirit."
            },
            new Author
            {
                Id=Guid.Parse("1af9840b-6115-41a2-b441-effe379fdc55"),
                Name="Martin Fowler",
                ImageUrl="/AuthorImages/Martin-Fowler.jpg",
                Biography="Martin Fowler is a British software engineer, author," +
                " and public speaker widely recognized for his contributions to software design, architecture," +
                " and development practices. Born in 1963 in Walsall, England, he studied at University College London, " +
                "where he earned a degree in Computer Science. Over the course of his career, " +
                "Fowler has become one of the most influential voices in modern software engineering.\r\n\r\n" +
                "Fowler is best known for his work on object-oriented analysis and design, particularly" +
                " his emphasis on writing clean, maintainable, and well-structured code. " +
                "He gained international recognition with the publication of his book " +
                "Refactoring: Improving the Design of Existing Code (1999), which introduced a systematic approach " +
                "to improving code quality without changing its external behavior. This book has had a lasting " +
                "impact on how developers approach code maintenance and evolution.\r\n\r\n" +
                "Another major contribution is his co-authorship of Patterns of Enterprise Application Architecture (2002)," +
                " where he cataloged common solutions to recurring problems in enterprise software systems. " +
                "Fowler also played a key role in popularizing the concept of Domain-Driven Design (DDD)" +
                " through his writings and collaborations with other thought leaders.\r\n\r\n" +
                "As Chief Scientist at ThoughtWorks, a global software consultancy, " +
                "Fowler has influenced large-scale software projects and promoted agile methodologies, " +
                "continuous integration, and evolutionary architecture. He is also a strong advocate for microservices, " +
                "helping define and clarify the concept in the early 2010s.\r\n\r\nThrough his website and blog, " +
                "martinfowler.com, he regularly publishes articles on software architecture," +
                " development practices, and technology trends. His clear writing style and practical insights have " +
                "made complex topics accessible to a wide audience of developers.\r\n\r\n" +
                "Martin Fowler’s work continues to shape how software is designed and built," +
                " encouraging developers to focus on simplicity, clarity, and continuous improvement."
            }
        };





        public void Configure(EntityTypeBuilder<Author> entity)
        {
            entity.HasData(initialAuthors);
        }
    }
}
