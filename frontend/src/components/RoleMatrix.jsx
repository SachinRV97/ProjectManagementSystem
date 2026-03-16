const rights = {
  Admin: ['All access'],
  'Portal-Admin': ['Manage portal for employees and customers'],
  'Portal-Employee': ['Manage customer records'],
  'Customer-Admin': ['Manage employees and users'],
  'Customer-Employee': ['Change portal design and content'],
  'Customer-User': ['Login/registration and normal app usage'],
};

export default function RoleMatrix() {
  return (
    <section className="panel">
      <h3>Role Access Matrix</h3>
      <ul>
        {Object.entries(rights).map(([role, capabilities]) => (
          <li key={role}>
            <strong>{role}</strong>: {capabilities.join(', ')}
          </li>
        ))}
      </ul>
    </section>
  );
}
