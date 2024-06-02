<table>
  <thead>
    <tr>
      <th>Term</th>
      <th>Definition</th>
      <th>Examples and Usage</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><strong>Wedding Event 💍</strong></td>
      <td>The core entity representing the wedding, including details like date, venue, guest list, and RSVP
        information.</td>
      <td>
        <ul>
          <li>In code: `WeddingEvent` class (Entity)</li>
          <li>In conversation: "We need to update the Wedding Event details with the new date."</li>
          <li>In documentation: "The Wedding Event Aggregate stores all relevant information about the wedding." </li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>RSVP 📅</strong></td>
      <td>A guest's response to the invitation, indicating whether they will attend or not, along with additional
        information like guest names and dietary restrictions.</td>
      <td>
        <ul>
          <li>In code: `RSVP` class (Entity)</li>
          <li>In conversation: "Have you received any new RSVPs today?"</li>
          <li>In API documentation: "The /rsvp endpoint accepts POST requests containing RSVP information."</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Guest 👤</strong></td>
      <td>An individual invited to the wedding, with associated information like name, email, phone number, and
        any special requests.</td>
      <td>
        <ul>
          <li>In code: `Guest` class (Entity)</li>
          <li>In conversation: "Can we get a list of all confirmed Guests?"</li>
          <li>In UI design: "The Guest Details page should display the guest's name, contact information, and RSVP
            status."</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Venue 📍</strong></td>
      <td>The location of the wedding ceremony and/or reception, including address, website, and contact
        information.</td>
      <td>
        <ul>
          <li>In code: `Venue` class (Entity)</li>
          <li>In conversation: "We need to add the Venue details to the website."</li>
          <li>In data model: "The Venue entity has attributes for name, address, and website URL."</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Accommodation 🛌</strong></td>
      <td>Information about hotel options or other lodging for guests attending the wedding.</td>
      <td>
        <ul>
          <li>In UI: "Accommodation" section on the website</li>
          <li>In conversation: "Have we finalized the list of recommended Accommodations for guests?"</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Gift Registry 🎁</strong></td>
      <td>Links to online services where guests can purchase gifts for the couple.</td>
      <td>
        <ul>
          <li>In UI: "Gift Registry" section on the website</li>
          <li>In conversation: "Please ensure the Gift Registry links are up-to-date." </li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Photo Gallery 📸</strong></td>
      <td>A collection of images and videos from the wedding, accessible to guests after the event.</td>
      <td>
        <ul>
          <li>In UI: "Photo Gallery" section (accessible after the wedding)</li>
          <li>In conversation: "We'll upload the photos to the Photo Gallery after the wedding."</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Chatbot 🤖</strong></td>
      <td>An AI-powered conversational interface that answers guest questions about the wedding.</td>
      <td>
        <ul>
          <li>In code: `ChatbotService` class</li>
          <li>In conversation: "The Chatbot can answer frequently asked questions about the wedding." </li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Chatbot Query ❓</strong></td>
      <td>A question or request from a guest to the chatbot.</td>
      <td>
        <ul>
          <li>In logs: "Received Chatbot Query: What time does the ceremony start?"</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Chatbot Response 💬</strong></td>
      <td>The chatbot's reply to a guest query, providing relevant information or assistance.</td>
      <td>
        <ul>
          <li>In chatbot interface: "The ceremony starts at 4:00 PM."</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Wedding Information Page 📝</strong></td>
      <td>The main page that provides comprehensive details about the wedding, including the couple's story,
        venue, date, and other relevant information.</td>
      <td>
        <ul>
          <li>In sitemap: "Wedding Information Page" </li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>RSVP Page 📅</strong></td>
      <td>The page where guests can submit their RSVP response.</td>
      <td>
        <ul>
          <li>In user testing: "Guests found the RSVP Page easy to use."</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Thank You Page 🙏</strong></td>
      <td>The page displayed after a guest submits their RSVP, expressing gratitude for their response.</td>
      <td>
        <ul>
          <li>In design mockups: "Thank You Page design"</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Contact Page 📞</strong></td>
      <td>The page providing contact information for the couple, allowing guests to reach out with questions or
        concerns.</td>
      <td>
        <ul>
          <li>In website footer: "Contact" link </li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Photo & Video Page 📸</strong></td>
      <td>The page where guests can view photos and videos from the wedding.</td>
      <td>
        <ul>
          <li>In post-wedding communications: "The Photo & Video Page is now live!" </li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Frontend 🎨</strong></td>
      <td>The visual presentation and interactive elements that guests experience when they visit the wedding
        website.</td>
      <td>
        <ul>
          <li>Example in Conversation: We need to ensure the Frontend is mobile-friendly so guests can easily RSVP from
            their phones.</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Backend ☁️</strong></td>
      <td>The hidden logic, processes, and data management that power the wedding website's functionality.
      </td>
      <td>
        <ul>
          <li>Example in Conversation: The Backend will handle storing and retrieving RSVPs securely.</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>Database 📈</strong></td>
      <td>The organized collection of all essential wedding-related information.</td>
      <td>
        <ul>
          <li>Example in Conversation: We'll store the venue's address and directions in the Database.</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><strong>CI/CD Pipeline 🚀</strong></td>
      <td>The automated process of integrating code changes, running tests, and deploying updates to the
        wedding website.</td>
      <td>
        <ul>
          <li>Example in Conversation: Our CI/CD Pipeline will deploy changes to the website every time we push code to
            the main branch, making sure the latest updates are always live for our guests.</li>
        </ul>
      </td>
    </tr>
  </tbody>
</table>