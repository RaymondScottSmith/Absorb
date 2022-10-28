using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControls : MonoBehaviour
{
    [SerializeField]
    private ForwardDir bowDirection;

    [SerializeField] private float frictionValue = 0.1f;

    [SerializeField] private float veerForce = 1f;
    
    [SerializeField] private Vector2 maxPositions = new Vector2(10,10);

    [SerializeField] private Vector2 minPositions = new Vector2(-10, -10);
    
    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Friction in space?
        myRB.velocity -= myRB.velocity * frictionValue;
        
        //Check within x bounds
        if (myRB.position.x < minPositions.x)
        {
            myRB.position = new Vector2(minPositions.x, myRB.position.y);
        }
        else if (myRB.position.x > maxPositions.x)
        {
            myRB.position = new Vector2(maxPositions.x, myRB.position.y);
        }
        
        //Check within y bounds
        if (myRB.position.y < minPositions.y)
        {
            myRB.position = new Vector2(myRB.position.x, minPositions.y);
        }
        else if (myRB.position.y > maxPositions.y)
        {
            myRB.position = new Vector2(myRB.position.x, maxPositions.y);
        }
        
        
    }

    public void HardPort()
    {
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.y, maxPositions.y) > 0.5f)
                    impDirection = Vector2.up;
                break;
            case ForwardDir.FUp:
                impDirection = Vector2.left;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }

    public void HardStarboard()
    {
        
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.y, minPositions.y) > 0.5f)
                    impDirection = Vector2.down;
                break;
            
            case ForwardDir.FUp:
                impDirection = Vector2.right;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }

    public void Accelerate()
    {
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.x, maxPositions.x) > 0.5f)
                    impDirection = Vector2.right;
                break;
            
            case ForwardDir.FUp:
                impDirection = Vector2.up;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }

    public void Decelerate()
    {
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.x, minPositions.x) > 0.5f)
                    impDirection = Vector2.left;
                break;
            
            case ForwardDir.FUp:
                impDirection = Vector2.down;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }
    
    private float Distance(float num1, float num2)
    {
        return Mathf.Abs(num1 - num2);
    }
    
    private enum ForwardDir
    {
        FRight,
        FLeft,
        FDown,
        FUp
    }
}